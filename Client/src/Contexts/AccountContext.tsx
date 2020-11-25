import React, { useState, useEffect, useRef } from "react"
import accountRepository from '../repository/AccountRepository'
interface IAccountContext {
    currentUserName: string,
    currentUserIdentificator:string,
    currentUserPassword:string,
    isLogged:boolean,
    login: (user: LoginModel) => Promise<boolean>,
    registration: (user: RegistrationModel) => Promise<boolean>,
    logout: () => void
}

export type LoginModel = {
    name: string,
    password: string
}

export type RegistrationModel = LoginModel & {
    email: string
}

export const AccountContext = React.createContext<IAccountContext>({
    currentUserName: '',
    currentUserIdentificator: '',
    currentUserPassword: '',
    isLogged:false,
    registration: (user: RegistrationModel) => {
        throw Error('Не проинициализирован контекст авторизации')
    },
    login: (user: LoginModel) => {
        throw Error('Не проинициализирован контекст авторизации')
    },
    logout: () => {
        throw Error('Не проинициализирован контекст авторизации')
    }
}) 

export const AccountContextProvider: React.FC = ({ children }) => {
    const savedUserName = localStorage.getItem('login')
    const savedUserIdentificator = localStorage.getItem('identificator')
    const savedUserPassword = localStorage.getItem('password')
    const [isLogged, setLogged] = useState(false)
    const [currentUserName, setUserName] = useState(savedUserName === null ? '' : savedUserName)
    const [currentUserIdentificator, setUserIdentificator] = useState(savedUserIdentificator === null ? '' : savedUserIdentificator)
    const [currentUserPassword, setUserPassword] = useState(savedUserPassword === null ? '' : savedUserPassword)
    let log = useRef({name: currentUserName, password: currentUserPassword})

    const login = async (user: LoginModel) => {
        let response: any
        try {
            response = await accountRepository.login(user).catch()
        }
        catch {
            return false
        }
        if (response.status === 200) {
            const json = await response.json()
            setUserName(json.name)
            setUserIdentificator(json.identificator)
            setUserPassword(json.password)
            setLogged(true)
            localStorage.setItem('login', json.name)
            localStorage.setItem('identificator', json.identificator)
            localStorage.setItem('password', json.password)
        }
        else {
            return false
        }
        return true
    }

    const logout = async () => {
        setUserName('')
        setUserIdentificator('')
        localStorage.removeItem('login')
        localStorage.removeItem('identificator')
        localStorage.removeItem('password')
        setLogged(false)
        await accountRepository.logout()
    }

    const registration = async (user: RegistrationModel) => {
        let response: any
        try {
            response = await accountRepository.registration(user).catch()
        }
        catch {
            return false
        }
        if (response.status === 200) {
            // TODO: обработка ответа регистрации
        }
        else {
            return false
        }
        return true
    }

    useEffect(() => {
        if(currentUserName !== '') {
            login(log.current)
        }
    },[])

    return (
        <AccountContext.Provider value={{currentUserName, currentUserIdentificator, currentUserPassword, isLogged, registration, login, logout}}>
            {children}
        </AccountContext.Provider>
    )
}