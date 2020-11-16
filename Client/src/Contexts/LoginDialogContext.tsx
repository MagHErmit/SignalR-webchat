import React, { useContext, useState, useRef } from 'react'
import { TextField, Button } from '@material-ui/core'
import '../styles/LoginDialog.css'
import { AccountContext } from './AccountContext'

interface ILoginDialogContext {
    setIsDialogOpen: (dialogOpen: boolean) => void
}

export const LoginDialogContext = React.createContext<ILoginDialogContext>({
    setIsDialogOpen: (dialogOpen: boolean) => {
        throw new Error('Не проинициализирован контект диалога')
    }
})

export const LogingDialogContextProvder: React.FC = ({ children }) => {
    const { login } = useContext(AccountContext)
    const dialogRef = useRef<HTMLDialogElement>(null)
    const inputRef = useRef<HTMLInputElement>(null)
    const [color, setColor] = useState<"secondary" | "primary" | undefined>("primary")
    const [text, setText] = useState('')
    const [disableLogin, setStateLogin] = useState(false)

    const setError = (color:"secondary" | "primary" | undefined, text:string) => {
        setColor(color)
        setText(text)
    }

    const setIsDialogOpen = (isDialogOpen: boolean) => {
        if (!inputRef.current || !dialogRef.current)
            return

        if (isDialogOpen) {
            dialogRef.current.showModal()
        }
        else {
            dialogRef.current.close()
        }
        setError("primary", '')
    }
    
    const loginInternal = async () => {
        
        if (!inputRef.current || !dialogRef.current)
            return
        let res = checkLogin(inputRef.current.value)
        if(res[0] == null) {
            setError("secondary", res[1] as string)
            return
        }

        setStateLogin(true)
        const loginRes = await login(res[0])
        setStateLogin(false)
        if(loginRes)
            dialogRef.current.close()
        else {
            setStateLogin(false)
            setError("secondary", 'Не удалось подключиться к серверу')
        }
    }

    return (
        <LoginDialogContext.Provider value={{ setIsDialogOpen }}>
            <dialog ref={dialogRef} className='login-dialog'>
                <div className='login-dialog-header'>
                    <span className='login-dialog-header-text'>Авторизация</span>                    
                </div>
                <div className='login-dialog-body'>
                    <TextField disabled={disableLogin} onKeyDown={(e) => {setError("primary",''); if (e.key === 'Enter') loginInternal() }}  inputRef={inputRef} label='Введите логин' fullWidth color={color}/>
                    <label style={{color: 'red'}}>{text}</label>
                </div>
                <div className='login-dialog-footer'>
                    <Button onClick={() => setIsDialogOpen(false)} className='login-dialog-close-button' color="primary">
                        Закрыть
                    </Button>
                    <Button disabled={disableLogin} onClick={() => loginInternal()} className='login-dialog-send-button' color="primary">
                        Вход
                    </Button>
                </div>
            </dialog>
            {children}
        </LoginDialogContext.Provider>
    )
}

function checkLogin(userName: string) {
    if(userName.length < 4 || userName.length > 20)
        return [null, 'Длина логина должна быть от 4 до 20 символов']
    if(!/^[a-zA-Z1-9]+$/.test(userName))
        return [null, 'В логине должны быть только латинские буквы или цифры']
    return [userName, null]
}