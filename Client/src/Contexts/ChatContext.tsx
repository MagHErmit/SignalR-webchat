import React, { useState, createContext, useEffect, useContext} from 'react'
import SignalRManager from '../SignalR/SignalRManager'
import { AccountContext } from './AccountContext';
import messagesRepository from '../repository/MessagesRepository'
import { DialogListContext } from './DialogListContext';
import { ConnectionContext, ConnectionStatus } from './ConnectionContext';


export type UserMessage = {
    userName: string,
    userId: string,
    chatId: number,
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
    const { dictChats, loaded, currentDialog, setCurrentDialog } = useContext(DialogListContext)
    const { status } = useContext(ConnectionContext)

    const getInitMessages = async (chatId: number) => {
        let response: any
        try {
            response = await messagesRepository.getMessages(chatId).catch()
        }
        catch {
            return 
        }
        if(response.status === 200) {
            const json = await response.json();
            return json
        }
    }

    const addMessage = (arr: UserMessage[], m: UserMessage) => {
        arr.push(m)
    }

    useEffect(() => {
        SignalRManager.instance.connection.on('ReciveFromServerMessage',(message: UserMessage) => {
            message.isMy = currentUserIdentificator === message.userId
            message.time = new Date().getTime()
            if(message.chatId === currentDialog)
                setMessages(existedMessages => [...existedMessages, message])
            addMessage(dictChats.getValue(message.chatId) as UserMessage[], message)
        })
        
        return () => {
            SignalRManager.instance.connection.off('ReciveFromServerMessage')
        }
    }, [currentUserIdentificator, currentDialog]) 

    useEffect(() => {
        if(isLogged && loaded) {
            dictChats.forEach((k, arr) => {
                getInitMessages(k).then(res => {
                    dictChats.setValue(k, res)
                })
            })
        }
        console.log(dictChats)
    },[isLogged, loaded])

    const updateMessages = async () => {
        dictChats.forEach(async (k, arr) => {
            await getInitMessages(k).then(res => {
                dictChats.setValue(k, res)
            })
        })
        
    }
    
    useEffect(() => {
        const obertka = async () => {
            await updateMessages()
        }
        if(status === ConnectionStatus.Alive) {
            obertka()
            if(currentDialog !== -1)
                setMessages((dictChats.getValue(currentDialog) as UserMessage[]))
        }
    },[status])

    useEffect(() => {
        if(currentDialog !== -1)
            setMessages(dictChats.getValue(currentDialog) as UserMessage[])
    }, [currentDialog])
    

    const sendMessage = (message: string, chatId: number) => {
        return SignalRManager.instance.connection.invoke('ReciveMessage', message, chatId).catch(() => {alert('Что-то пошло не так...')})
    }

    return <ChatContext.Provider value={{ messages, sendMessage }}>
        {children}
    </ChatContext.Provider>
}