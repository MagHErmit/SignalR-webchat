import React, { useState } from "react"
import accountRepository from '../repository/AccountRepository'
interface IAccountContext {
    currentUserName: string,
    currentUserIdentificator:string,
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
    const [currentUserName, setUserName] = useState(savedUserName === null ? '' : savedUserName)
    const [currentUserIdentificator, setUserIdentificator] = useState(savedUserIdentificator === null ? '' : savedUserIdentificator)

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
            localStorage.setItem('login', json.name)
            localStorage.setItem('identificator', json.identificator)
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
            
        }
        else {
            return false
        }
        return true
    }

    return (
        <AccountContext.Provider value={{currentUserName, currentUserIdentificator, registration, login, logout}}>
            {children}
        </AccountContext.Provider>
    )
}