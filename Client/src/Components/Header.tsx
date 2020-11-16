import React, { useContext, useEffect, useState } from 'react'
import { LoginDialogContext } from '../Contexts/LoginDialogContext'
import '../styles/Header.css'
import { AccountContext } from '../Contexts/AccountContext'
import { ConnectionContext, ConnectionStatus } from '../Contexts/ConnectionContext'

const Header: React.FC = () => {
    const { currentUserName, logout } = useContext(AccountContext)
    const { setIsDialogOpen } = useContext(LoginDialogContext)
    const { status } = useContext(ConnectionContext)
    const [message, setMessage] = useState('')

    useEffect(() => {
        switch(status) {
            case ConnectionStatus.Dead: {
                setMessage('Не удалось подключиться к серверу')
                break
            }
            case ConnectionStatus.Process: {
                setMessage('Подключение к серверу...')
                break
            }
            default: {
                setMessage('')
                break
            }
        }
    },[status])

    const getLoginBlock = () => {

        if (currentUserName === '') {
            return (
                <div className={'unauthorized-user'}>
                    <span className={'connection-text'}>{message}{/*status*/}</span>
                    <span className={'navbar-item login-button'} onClick={() => setIsDialogOpen(true)}>Войти</span>
                </div>
                    )
        }

        return (
            <div className={'authorized-user'}>
                <span className={'connection-text'}>{message}{/*status*/}</span> 
                <span>
                    <span className={'user-name'}>{currentUserName}</span>
                    <span className={'navbar-item'} onClick={logout }>Выход</span>
                </span>
                
            </div>
        )
    }

    return (
        <header>
            <div className="navbar-brand">MyСhat</div>
            {getLoginBlock()}
        </header>
    )
}

export default Header;