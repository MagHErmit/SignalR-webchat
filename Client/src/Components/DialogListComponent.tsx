import React, { useContext, useEffect, useState } from 'react'
import { DialogListContext, DialogListContextProvider } from '../Contexts/DialogListContext'

interface IDialogBlockProps {
    name: string
}


const DialogBlockComponent: React.FC<IDialogBlockProps> = ({name}) => {
    return <div onClick={() => {alert('robit')}}>{name}</div>
}

export const DialogListComponent: React.FC = () => {
    const { dialogs } = useContext(DialogListContext)
    
    useEffect(() => {
        console.log(dialogs)
    },[dialogs])

    return (
        
            <div>диалоги  
                {
                    dialogs.map((c) => <DialogBlockComponent name={c.name}/>)
                }
            </div>
        
    )
}


export const DialogComponent: React.FC = () => {
    return (
        <DialogListContextProvider>
            <DialogListComponent />
        </DialogListContextProvider>
    )
}