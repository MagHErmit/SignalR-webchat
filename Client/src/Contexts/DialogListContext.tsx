import { Button, TextField } from "@material-ui/core";
import React, { createContext, useContext, useEffect, useRef, useState } from "react"
import dialogsRepository from "../repository/DialogsRepository";
import { AccountContext } from "./AccountContext"
import { UserMessage } from "./ChatContext"
import * as Collections from 'typescript-collections'

type Dialog = {
    id: number,
    name: string,
    userCreatorId: string
}

interface IDialogListContext {
    dialogs: Dialog[],
    currentDialog: number,
    currentChat: UserMessage[],
    dictChats: Collections.Dictionary<number, UserMessage[]>,
    loaded: boolean,
    setCurrentDialog: (chatId: number) => void
    setIschatDialogNameOpen: (isDialogOpen: boolean) => void
    appendMessage: (message: UserMessage) => void
}


export const DialogListContext = createContext<IDialogListContext>({
    dialogs: [],
    currentDialog: -1,
    currentChat: [],
    loaded: false,
    dictChats: new Collections.Dictionary<number, UserMessage[]>(),
    setCurrentDialog: (chatId: number) => {
        throw new Error("Контекст примера не проинициализирован")
    },
    setIschatDialogNameOpen: (isDialogOpen: boolean) => {
        throw new Error("Контекст диалогов не проинициализирован")
    },
    appendMessage: (message: UserMessage) => {
        throw new Error("Контекст диалогов не проинициализирован")
    }
});

export const DialogListContextProvider: React.FC = ({children}) => {
    const { isLogged } = useContext(AccountContext)
    const [dialogs, setDialogs] = useState<Dialog[]>([])
    const [currentDialog, setCurrentDialogInternal] = useState(-1)
    const chatDialogNameRef = useRef<HTMLDialogElement>(null)
    const inputChatNameRef = useRef<HTMLInputElement>(null)
    const { dictChats } = useContext(DialogListContext)
    const [currentChat, setCurrentChatInternal] = useState<UserMessage[]>([])

    const [loaded, setter] = useState(false)

    const createChat = async () => {
        if(!inputChatNameRef.current || !chatDialogNameRef.current)
            return
        let response: any;
        try {
            response = await dialogsRepository.createDialog(inputChatNameRef.current.value)
        }
        catch {
            return
        }
        const json = await response.json()
        setDialogs(existedDialogs => [...existedDialogs, json])
        dictChats.setValue(json.id, [])
        chatDialogNameRef.current.close()
    }

    const setIschatDialogNameOpen = (isDialogOpen: boolean) => {
        if (!chatDialogNameRef.current)
            return

        if (isDialogOpen) {
            chatDialogNameRef.current.showModal()
        }
        else {
            chatDialogNameRef.current.close()
        }
    }

    const setCurrentDialog = (chatId: number) => {
        setCurrentDialogInternal(chatId)
        setCurrentChatInternal(dictChats.getValue(chatId) as UserMessage[])
    }

    const appendMessage = (message: UserMessage) => {
        setCurrentChatInternal(existedMessages => [...existedMessages, message])
    }

    const getDialogs = async () => {
        let response: any
        try {
            response = await dialogsRepository.getDialogs().catch()
        }
        catch {
            return
        }
        if(response.status === 200) {
            const json = await response.json()
            setDialogs(json)
            json.forEach((e: { id: number; }) => {dictChats.setValue(e.id, [])})
        }
        setter(true)
    }

    useEffect(() => {
        console.log(currentDialog)
    }, [currentDialog])
    
    useEffect(() => {
        if(isLogged)
            getDialogs()
    }, [isLogged])



    return <DialogListContext.Provider value={{dialogs, currentDialog, currentChat, loaded, appendMessage, setCurrentDialog, setIschatDialogNameOpen, dictChats}}>
        <dialog ref={chatDialogNameRef} className='login-dialog'>
                <div className='login-dialog-header'>
                    <span className='login-dialog-header-text'>Создание чата</span>                    
                </div>
                <div className='login-dialog-body'>
                    <TextField inputRef={inputChatNameRef} label='Введите название чата' fullWidth/>
                    <label style={{color: 'red'}}></label>
                </div>
                <div className='login-dialog-footer'>
                    <Button onClick={() => {setIschatDialogNameOpen(false)}} className='login-dialog-close-button'>
                        Закрыть
                    </Button>
                    <Button onClick={() => {createChat()}} className='login-dialog-send-button' color="primary">
                        Создать
                    </Button>
                </div>
            </dialog>
        {children}
    </DialogListContext.Provider>
}