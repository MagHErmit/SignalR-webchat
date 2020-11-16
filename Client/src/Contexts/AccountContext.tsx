import React, { useState } from "react"
import accountRepository from '../repository/AccountRepository'
interface IAccountContext {
    currentUserName: string,
    currentUserIdentificator:string,
    login: (name: string) => Promise<boolean>,
    logout: () => void
}

export const AccountContext = React.createContext<IAccountContext>({
    currentUserName: '',
    currentUserIdentificator: '', 
    login: (name: string) => {
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

    const login = async (userName: string) => {
        let response: any
        try {
            response = await accountRepository.login(userName).catch()
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

    return (
        <AccountContext.Provider value={{currentUserName, currentUserIdentificator, login, logout}}>
            {children}
        </AccountContext.Provider>
    )
}