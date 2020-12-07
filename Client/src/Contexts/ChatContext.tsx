import React, { useState, createContext, useEffect, useContext} from 'react'
import SignalRManager from '../SignalR/SignalRManager'
import { AccountContext } from './AccountContext';
import messagesRepository from '../repository/MessagesRepository'

type UserMessage = {
    userName: string,
    userId: string,
    text: string,
    isMy: boolean,
    time: number
}
interface IChatContext {
    messages: UserMessage[],
    sendMessage: (message: string, chatId: number) => Promise<void>
}

export const ChatContext = createContext<IChatContext>({
    messages: [],
    sendMessage: (message: string, chatId: number) => {
        throw new Error("Контекст примера не проинициализирован")
    }
});

export const ChatContextProvider: React.FC = ({children}) => {
    const [messages, setMessages] = useState<UserMessage[]>([])
    const { currentUserIdentificator, isLogged } = useContext(AccountContext)
    const getInitMessages = async () => {
        let response: any
        try {
            response = await messagesRepository.getMessages().catch()
        }
        catch {
            return 
        }
        if(response.status === 200) {
            const json = await response.json();
            setMessages(json)
        }
    }
    
    useEffect(() => {
        SignalRManager.instance.connection.on('ReciveFromServerMessage',(message: UserMessage) => {
            message.isMy = currentUserIdentificator === message.userId
            message.time = new Date().getTime()

            setMessages(existedMessages => [...existedMessages, message])
        })
        
        return () => {
            SignalRManager.instance.connection.off('ReciveFromServerMessage')
        }
    }, [currentUserIdentificator]) 

    useEffect(() => {
        if(isLogged)
            getInitMessages()
    },[isLogged])
    
    

    const sendMessage = (message: string, chatId: number) => {
        return SignalRManager.instance.connection.invoke('ReciveMessage', message, chatId).catch(() => {alert('Что-то пошло не так...')})
    }

    return <ChatContext.Provider value={{ messages, sendMessage }}>
        {children}
    </ChatContext.Provider>
}