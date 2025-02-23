import React, { createContext, useCallback, useContext, useEffect, useRef, useState } from 'react'
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
    const { currentUserName } = useContext(AccountContext)

    const stopConnection = () => {
        SignalRManager.instance.stop()
        setStatus(ConnectionStatus.None)
    }

    const timerId = useRef<NodeJS.Timeout>()

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
        if(currentUserName !== '')
            startConnection()
        else if(!SignalRManager.instance.isDisconnected()) {
            stopConnection()
        }
        
    },[currentUserName])
    
    return (<ConnectionContext.Provider value={{status}}>
        {children}
    </ConnectionContext.Provider>
    )
}

