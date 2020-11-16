import React, { useState, createContext, useEffect, useContext, useRef } from 'react'
import SignalRManager from '../SignalR/SignalRManager'
import { AccountContext } from './AccountContext';

type UserMessage = {
    userName: string,
    userIdentificator: string,
    text: string,
    isMy: boolean,
    time: number
}
interface IChatContext {
    messages: UserMessage[],
    sendMessage: (message: string) => Promise<void>
}

export const ChatContext = createContext<IChatContext>({
    messages: [],
    sendMessage: (message: string) => {
        throw new Error("Контекст примера не проинициализирован")
    }
});

export const ChatContextProvider: React.FC = ({children}) => {
    const [messages, setMessages] = useState<UserMessage[]>([])
    const { currentUserName, currentUserIdentificator } = useContext(AccountContext)
    useEffect(() => {
        SignalRManager.instance.connection.on('ReciveFromServerMessage',(message: UserMessage) => {
            message.isMy = currentUserIdentificator === message.userIdentificator
            message.time = new Date().getTime()

            setMessages(existedMessages => [...existedMessages, message])
        })

        return () => {
            SignalRManager.instance.connection.off('ReciveFromServerMessage')
        }
    }, [currentUserName, currentUserIdentificator]) 

    const sendMessage = (message: string) => {
        return SignalRManager.instance.connection.invoke('ReciveMessage', message).catch(() => {alert('Что-то пошло не так...')})
    }

    return <ChatContext.Provider value={{ messages, sendMessage }}>
        {children}
    </ChatContext.Provider>
}