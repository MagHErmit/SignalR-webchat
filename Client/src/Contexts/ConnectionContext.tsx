import React, { createContext, useContext, useEffect, useRef, useState } from 'react'
import SignalRManager from '../SignalR/SignalRManager'
import { AccountContext } from './AccountContext'

export enum ConnectionStatus {
    None,
    Dead,
    Process,
    Alive
}
interface IConnectionContext{
    status: ConnectionStatus
}

export const ConnectionContext = createContext<IConnectionContext>({
    status: ConnectionStatus.None
})

export const ConnectionContextProvider: React.FC = ({children}) => {
    const connectionInterval = 5000

    const [status, setStatus] = useState(ConnectionStatus.None)
    const { isLogged } = useContext(AccountContext)
    const timerId = useRef<NodeJS.Timeout>()
    
    const stopConnection = () => {
        clearInterval(timerId.current as NodeJS.Timeout)
        SignalRManager.instance.stop()
        setStatus(ConnectionStatus.None)
    }

    const startConnection = () => {
        timerId.current = setInterval(() => {
            setStatus(ConnectionStatus.Process)
            SignalRManager.instance.start().then(() => {
                setStatus(ConnectionStatus.Alive)
                clearInterval(timerId.current as NodeJS.Timeout)
            }).catch(() => {
                setStatus(ConnectionStatus.Dead)
                if(!SignalRManager.instance.isDisconnected()) clearInterval(timerId.current as NodeJS.Timeout)
            })
        }, connectionInterval)
    }

    useEffect(() => {
        SignalRManager.instance.connection.onclose(() => {
            setStatus(ConnectionStatus.None)
        })
        SignalRManager.instance.connection.onreconnecting(() => {
            setStatus(ConnectionStatus.Process)
        })
        SignalRManager.instance.connection.onreconnected(() => {
            setStatus(ConnectionStatus.Alive)
        })
    }, [])

    useEffect(() => {
        if(isLogged) {
            startConnection()
        }
        else if(!SignalRManager.instance.isDisconnected()) {
            stopConnection()
        }
    },[isLogged])
    
    return (<ConnectionContext.Provider value={{status}}>
        {children}
    </ConnectionContext.Provider>
    )
}

