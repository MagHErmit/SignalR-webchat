import React, { createContext, useContext, useEffect, useState } from "react"
import dialogsRepository from "../repository/DialogsRepository";
import SignalRManager from "../SignalR/SignalRManager"
import { AccountContext } from "./AccountContext"

type Dialog = {
    id: number,
    name: string,
    userCreatorId: string
}

interface IDialogListContext {
    dialogs: Dialog[]
}


export const DialogListContext = createContext<IDialogListContext>({
    dialogs: []
});

export const DialogListContextProvider: React.FC = ({children}) => {
    const { isLogged } = useContext(AccountContext)
    const [dialogs, setDialogs] = useState<Dialog[]>([])

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
        if(isLogged)
            getDialogs()
    }, [isLogged])


    return <DialogListContext.Provider value={{dialogs}}>
        {children}
    </DialogListContext.Provider>
}