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
    dictChats: Collections.Dictionary<number, UserMessage[]>,
    loaded: boolean,
    setCurrentDialog: (chatId: number) => void,
    setIschatDialogNameOpen: (isDialogOpen: boolean) => void,
    setIsAddUserDialogOpen: (isDialogOpen: boolean) => void,
}


export const DialogListContext = createContext<IDialogListContext>({
    dialogs: [],
    currentDialog: -1,
    loaded: false,
    dictChats: new Collections.Dictionary<number, UserMessage[]>(),
    setCurrentDialog: (chatId: number) => {
        throw new Error("Контекст примера не проинициализирован")
    },
    setIschatDialogNameOpen: (isDialogOpen: boolean) => {
        throw new Error("Контекст диалогов не проинициализирован")
    },
    setIsAddUserDialogOpen: (isDialogOpen: boolean) => {
        throw new Error("Контекст диалогов не проинициализирован")
    }
});

export const DialogListContextProvider: React.FC = ({children}) => {
    const { isLogged } = useContext(AccountContext)
    const [dialogs, setDialogs] = useState<Dialog[]>([])
    const [currentDialog, setCurrentDialogInternal] = useState(-1)
    const chatDialogNameRef = useRef<HTMLDialogElement>(null)
    const inputChatNameRef = useRef<HTMLInputElement>(null)

    const addUserToChatDialogNameRef = useRef<HTMLDialogElement>(null)
    const inputUserChatNameRef = useRef<HTMLInputElement>(null)

    const { dictChats } = useContext(DialogListContext)

    const [loaded, setLoadedStatus] = useState(false)

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
        if (!chatDialogNameRef.current || !isLogged)
            return

        if (isDialogOpen) {
            chatDialogNameRef.current.showModal()
        }
        else {
            chatDialogNameRef.current.close()
        }
    }

    const setIsAddUserDialogOpen = (isDiaolgOpen: boolean) => {
        if(!addUserToChatDialogNameRef.current || !inputUserChatNameRef.current || currentDialog === -1)
            return
        if(isDiaolgOpen) {
            addUserToChatDialogNameRef.current.showModal()
        }
        else {
            addUserToChatDialogNameRef.current.close()
        }
    }

    const setCurrentDialog = (chatId: number) => {
        setCurrentDialogInternal(chatId)
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
        setLoadedStatus(true)
    }

    const addUserToChat = async () => {
        if(inputUserChatNameRef.current !== null && addUserToChatDialogNameRef.current !== null) {
            let response: any
            try {
                response = await dialogsRepository.addUserToDialog(inputUserChatNameRef.current.value, currentDialog).catch()
            }
            catch {
                return false
            }
            if(response.status === 200) {
                addUserToChatDialogNameRef.current.close()
                return true
                
            }
        }
        return false
    }

    useEffect(() => {
        if(isLogged)
            getDialogs()
    }, [isLogged])



    return <DialogListContext.Provider value={{dialogs, currentDialog, loaded, setCurrentDialog, setIschatDialogNameOpen, setIsAddUserDialogOpen, dictChats}}>
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
        <dialog ref={addUserToChatDialogNameRef} className='login-dialog'>
                <div className='login-dialog-header'>
                    <span className='login-dialog-header-text'>Добавить пользователя</span>                    
                </div>
                <div className='login-dialog-body'>
                    <TextField inputRef={inputUserChatNameRef} label='Введите имя пользователя' fullWidth/>
                    <label style={{color: 'red'}}></label>
                </div>
                <div className='login-dialog-footer'>
                    <Button onClick={() => {setIsAddUserDialogOpen(false)}} className='login-dialog-close-button'>
                        Закрыть
                    </Button>
                    <Button onClick={() => {addUserToChat()}} className='login-dialog-send-button' color="primary">
                        Добавить
                    </Button>
                </div>
        </dialog>
        {children}
    </DialogListContext.Provider>
}