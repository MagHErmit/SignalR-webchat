import React, { useContext, useEffect} from 'react'
import { DialogListContext } from '../Contexts/DialogListContext'
import '../styles/DialogList.css'

interface IDialogBlockProps {
    id: number,
    name: string
}

const DialogBlockComponent: React.FC<IDialogBlockProps> = ({id, name}) => {
    const { setCurrentDialog } = useContext(DialogListContext)

    return <div className='dialogButton' onClick={() => {setCurrentDialog(id)}}>{name}</div>
}

export const DialogListComponent: React.FC = () => {
    const { dialogs } = useContext(DialogListContext)
    
    return (
        <div className='dialog-list-container'>
            {
                dialogs.map((c) => <DialogBlockComponent id={c.id} name={c.name}/>)
            }
        </div>
    )
}


export const DialogsComponent: React.FC = () => {
    return (
        <DialogListComponent />
    )
}