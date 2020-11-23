import React, { useContext, useState, useRef } from 'react'
import { TextField, Button } from '@material-ui/core'
import '../styles/LoginDialog.css'
import { AccountContext, RegistrationModel } from './AccountContext'
import md5 from 'md5'


interface IRegistrationDialogContext {
    setIsRegistrationDialogOpen: (dialogOpen: boolean) => void
}

export const RegistrationDialogContext = React.createContext<IRegistrationDialogContext>({
    setIsRegistrationDialogOpen: (dialogOpen: boolean) => {
        throw new Error('Не проинициализирован контект диалога')
    }
})

export const RegistrationDialogContextProvider: React.FC = ({ children }) => {
    const { registration } = useContext(AccountContext)
    const dialogRef = useRef<HTMLDialogElement>(null)
    const inputLoginRef = useRef<HTMLInputElement>(null)
    const inputEmailRef = useRef<HTMLInputElement>(null)
    const inputPasswordRef = useRef<HTMLInputElement>(null)
    const [color, setColor] = useState<"secondary" | "primary" | undefined>("primary")
    const [text, setText] = useState('')
    const [disableLogin, setStateLogin] = useState(false)

    const setError = (color:"secondary" | "primary" | undefined, text:string) => {
        setColor(color)
        setText(text)
    }

    const setIsRegistrationDialogOpen = (isDialogOpen: boolean) => {
        if (!inputLoginRef.current || !dialogRef.current)
            return

        if (isDialogOpen) {
            dialogRef.current.showModal()
        }
        else {
            dialogRef.current.close()
        }
        setError("primary", '')
    }
    
    const registrationInternal = async () => {
        
        if (!inputLoginRef.current || !dialogRef.current)
            return
        let res = checkLogin(inputLoginRef.current.value)
        if(res[0] == null) {
            setError("secondary", res[1] as string)
            return
        }

        setStateLogin(true)
        let RegistrationRes: any
        if(inputEmailRef.current && inputPasswordRef.current) {
            let reg: RegistrationModel = {name: res[0], email: inputEmailRef.current.value, password: md5(inputPasswordRef.current.value)}
            RegistrationRes = await registration(reg)
        }
        setStateLogin(false)
        if(RegistrationRes)
            dialogRef.current.close()
        else {
            setStateLogin(false)
            setError("secondary", 'Не удалось подключиться к серверу')
        }
    }

    return (
        <RegistrationDialogContext.Provider value={{ setIsRegistrationDialogOpen }}>
            <dialog ref={dialogRef} className='login-dialog'>
                <div className='login-dialog-header'>
                    <span className='login-dialog-header-text'>Регистрация</span>                    
                </div>
                <div className='login-dialog-body'>
                    <TextField disabled={disableLogin} onKeyDown={(e) => {setError("primary",''); if (e.key === 'Enter') registrationInternal() }}  inputRef={inputLoginRef} label='Введите логин' fullWidth color={color}/>
                    <label style={{color: 'red'}}></label>
                </div>
                <div className='login-dialog-body'>
                    <TextField disabled={disableLogin} onKeyDown={(e) => {setError("primary",''); if (e.key === 'Enter') registrationInternal() }}  inputRef={inputEmailRef} label='Введите email' fullWidth color={color}/>
                    <label style={{color: 'red'}}></label>
                </div>
                <div className='login-dialog-body'>
                    <TextField disabled={disableLogin} type='password' onKeyDown={(e) => {setError("primary",''); if (e.key === 'Enter') registrationInternal() }}  inputRef={inputPasswordRef} label='Введите пароль' fullWidth color={color}/>
                    <label style={{color: 'red'}}>{text}</label>
                </div>
                <div className='login-dialog-footer'>
                    <Button onClick={() => setIsRegistrationDialogOpen(false)} className='login-dialog-close-button' color="primary">
                        Закрыть
                    </Button>
                    <Button disabled={disableLogin} onClick={() => registrationInternal()} className='login-dialog-send-button' color="primary">
                        Регистрация
                    </Button>
                </div>
            </dialog>
            {children}
        </RegistrationDialogContext.Provider>
    )
}

function checkLogin(userName: string) {
    if(userName.length < 4 || userName.length > 20)
        return [null, 'Длина логина должна быть от 4 до 20 символов']
    if(!/^[a-zA-Z1-9]+$/.test(userName))
        return [null, 'В логине должны быть только латинские буквы или цифры']
    return [userName, null]
}