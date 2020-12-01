import React, { useState, useEffect, useRef } from "react"
import accountRepository from '../repository/AccountRepository'
interface IAccountContext {
    currentUserName: string,
    currentUserIdentificator:string,
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
    const expiresUtc = localStorage.getItem('expiresUtc')
    const [isLogged, setLogged] = useState(false)
    const [currentUserName, setUserName] = useState(savedUserName === null || !checkCookiesExpire(expiresUtc) ? '' : savedUserName)
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
            setLogged(true)
            localStorage.setItem('login', json.name)
            localStorage.setItem('identificator', json.identificator)
            localStorage.setItem('expiresUtc', json.expiresUtc)
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
        localStorage.removeItem('expiresUtc')
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
            setLogged(true)
        }
    },[])

    return (
        <AccountContext.Provider value={{currentUserName, currentUserIdentificator, isLogged, registration, login, logout}}>
            {children}
        </AccountContext.Provider>
    )
}

function checkCookiesExpire(item: string | null): boolean {

    if (item == null)
        return false

    //const object = JSON.parse(item)
    const expireDate = Date.parse(item)//object.expireDate)
    if (expireDate > Date.now())
        return true

    return false
}