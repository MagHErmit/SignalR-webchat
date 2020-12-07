import React, { createContext, useContext, useEffect, useState } from "react"
import dialogsRepository from "../repository/DialogsRepository";
import { AccountContext } from "./AccountContext"

type Dialog = {
    id: number,
    name: string,
    userCreatorId: string
}

interface IDialogListContext {
    dialogs: Dialog[],
    currentDialog: number,
    setCurrentDialog: (chatId: number) => void
}


export const DialogListContext = createContext<IDialogListContext>({
    dialogs: [],
    currentDialog: -1,
    setCurrentDialog: (chatId: number) => {
        throw new Error("Контекст примера не проинициализирован")
    }
});

export const DialogListContextProvider: React.FC = ({children}) => {
    const { isLogged } = useContext(AccountContext)
    const [dialogs, setDialogs] = useState<Dialog[]>([])
    const [currentDialog, setCurrentDialogInternal] = useState(7)

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
            const json = await response.json();
            setDialogs(json)
        }
    }

    useEffect(() => {
        console.log(currentDialog)
    }, [currentDialog])

    useEffect(() => {
        if(isLogged)
            getDialogs()
    }, [isLogged])


    return <DialogListContext.Provider value={{dialogs, currentDialog, setCurrentDialog}}>
        {children}
    </DialogListContext.Provider>
}