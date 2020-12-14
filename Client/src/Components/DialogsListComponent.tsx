import React, { useContext, useEffect} from 'react'
import { AccountContext } from '../Contexts/AccountContext'
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
    const { dialogs, setIschatDialogNameOpen, setIsAddUserDialogOpen} = useContext(DialogListContext)

    return (
        <div className='dialog-list-container'>
            <div className='buttons-panel'>
                <span onClick={() => {setIschatDialogNameOpen(true)}} ><svg className='icon' xmlns="http://www.w3.org/2000/svg" height="auto" width="30%" viewBox="0 0 512 512" ><path d="m256 0c-141.164062 0-256 114.835938-256 256s114.835938 256 256 256 256-114.835938 256-256-114.835938-256-256-256zm112 277.332031h-90.667969v90.667969c0 11.777344-9.554687 21.332031-21.332031 21.332031s-21.332031-9.554687-21.332031-21.332031v-90.667969h-90.667969c-11.777344 0-21.332031-9.554687-21.332031-21.332031s9.554687-21.332031 21.332031-21.332031h90.667969v-90.667969c0-11.777344 9.554687-21.332031 21.332031-21.332031s21.332031 9.554687 21.332031 21.332031v90.667969h90.667969c11.777344 0 21.332031 9.554687 21.332031 21.332031s-9.554687 21.332031-21.332031 21.332031zm0 0"/></svg></span>
                <span onClick={() => {setIsAddUserDialogOpen(true)}} ><svg className='icon' xmlns="http://www.w3.org/2000/svg" height="auto" width="30%" viewBox="0 0 512 512" ><path d="m256 0c-141.164062 0-256 114.835938-256 256s114.835938 256 256 256 256-114.835938 256-256-114.835938-256-256-256zm112 277.332031h-90.667969v90.667969c0 11.777344-9.554687 21.332031-21.332031 21.332031s-21.332031-9.554687-21.332031-21.332031v-90.667969h-90.667969c-11.777344 0-21.332031-9.554687-21.332031-21.332031s9.554687-21.332031 21.332031-21.332031h90.667969v-90.667969c0-11.777344 9.554687-21.332031 21.332031-21.332031s21.332031 9.554687 21.332031 21.332031v90.667969h90.667969c11.777344 0 21.332031 9.554687 21.332031 21.332031s-9.554687 21.332031-21.332031 21.332031zm0 0"/></svg></span>
            </div>
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