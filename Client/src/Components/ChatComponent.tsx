import React, { useRef, useCallback, useContext, useMemo, useEffect } from 'react'
import '../styles/Chat.css'
import  { ChatContext, ChatContextProvider } from '../Contexts/ChatContext'
import { ConnectionContext, ConnectionStatus } from '../Contexts/ConnectionContext'
import { AccountContext } from '../Contexts/AccountContext'
import { DialogListContext } from '../Contexts/DialogListContext'

interface IMessageBlockProps {
    userName: string,
    isMy: boolean,
    text: string
}

const ChatMessageListComponent: React.FC = () => {
    const { messages } = useContext(ChatContext)
    const bottomRef = useRef<HTMLDivElement>(null)
    const scrollToBottom = () => {
        if (!bottomRef.current) return
        bottomRef.current.scrollIntoView({
            behavior: 'smooth',
            block: 'start',
        })
    }
    useEffect(() => {
        scrollToBottom()
    }, [messages])

    return (
        <div className='message-list'>
            {
               messages.map((m) => <ChatMessagesBlockComponent key={m.time} isMy={m.isMy} userName={m.userName} text={m.text}/>)
            }
            <div ref={bottomRef}> </div>
        </div>
    )
}

const ChatMessagesBlockComponent: React.FC<IMessageBlockProps> = ({ userName, isMy, text }) => {
    
    const messageRowClasses = `message-row ${isMy ? 'message-row-my' : 'message-row-another'}`
    const userLogoClasses = `user-logo ${isMy ? 'user-logo-my' : 'user-logo-another'}`
    const messageTextClasses = `message ${isMy ? 'my-message' : 'another-message'}`

    const userInitials = useMemo(() => {
        return (userName[0] + userName[1]).toUpperCase()
    },[userName])

    const stringToColour = useCallback((): string => {
        let hash = 0;
        for (let i = 0; i < userInitials.length; i++) {
          hash = userInitials.charCodeAt(i) + ((hash << 5) - hash);
        }
        let colour = '#';
        for (let i = 0; i < 3; i++) {
          let value = (hash >> (i * 7)) & 0xFF;
          colour += ('00' + value.toString(16)).substr(-2);
        }
        return colour;
    }, [userInitials])

    const style = {
        'backgroundColor': stringToColour()
    }

    return (
        <span className={messageRowClasses}>
            {
                isMy 
                ?
                    <>
                        <span className={messageTextClasses}>{text}</span>
                        <span style={style} className={userLogoClasses}>{userInitials}</span>
                        
                    </>
                : 
                    <>
                        <span style={style} className={userLogoClasses}>{userInitials}</span>
                        <span className={messageTextClasses}>{text}</span>
                    </>
            }
        </span>
    )
}

const ChatInputBlockComponent: React.FC = () => {
    const { sendMessage } = useContext(ChatContext)
    const { currentDialog } = useContext(DialogListContext)
    const { status } = useContext(ConnectionContext)
    const textAreaRef = useRef<HTMLTextAreaElement>(null)
    const { currentUserName } = useContext(AccountContext)

    const onEnter = useCallback(async (event: React.KeyboardEvent<HTMLTextAreaElement>) => {

        if (event.keyCode === 13 && textAreaRef.current) {
            event.preventDefault()

            if (textAreaRef.current.value !== '') {
                await sendMessage(textAreaRef.current.value, currentDialog)
                console.log(textAreaRef.current.value)
                textAreaRef.current.value = ''
            }
        }
    }, [textAreaRef, currentDialog])

    useEffect(() => {
        if (textAreaRef.current !== null) {
            textAreaRef.current.value = ''
        }
    }, [currentUserName])
    return (
        <div className='input-block'>
            <textarea ref={textAreaRef} className='text-input' placeholder={'Введите сообщение'} rows={1} onKeyDown={(event) => onEnter(event)} disabled={status !== ConnectionStatus.Alive}/>
        </div>
    )
}

export const ChatComponent: React.FC = () => {
    return (
        <ChatContextProvider>
            <div className='chat-container'>
                <ChatMessageListComponent />
                <ChatInputBlockComponent />
            </div>
        </ChatContextProvider>
    )
}