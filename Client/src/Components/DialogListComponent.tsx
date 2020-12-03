import React, { useContext, useEffect, useState } from 'react'
import { AccountContext } from '../Contexts/AccountContext'
import dialogsRepository from '../repository/DialogsRepository'


interface IChatBlockProps {
    id: number,
    name: string,
    userCreatorId: string
}

type Chat = {
    id: number,
    name: string,
    userCreatorId: string
}

const DialogBlockComponent: React.FC<IChatBlockProps> = ({id, name, userCreatorId}) => {
    return <span><span>{name}</span></span>
}

export const DialogListComponent: React.FC = () => {
    const { isLogged } = useContext(AccountContext)

    const [dialogs, setDialogs] = useState<Chat[]>([])

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

    return (
        <div>диалоги  
            {
                dialogs.map((c) => <DialogBlockComponent id={c.id} name={c.name} userCreatorId={c.userCreatorId} />)
            }
        </div>
    )
}