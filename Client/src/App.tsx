import React from 'react'
import './App.css'
import Header from './Components/Header'
import { ChatComponent } from './Components/ChatComponent'
import { AccountContextProvider } from './Contexts/AccountContext'
import { LogingDialogContextProvder } from './Contexts/LoginDialogContext'
import { ConnectionContextProvider } from './Contexts/ConnectionContext'

function App() {
  return (
    <AccountContextProvider>
      <ConnectionContextProvider>
        <LogingDialogContextProvder>
            <Header />
        </LogingDialogContextProvder>
        <ChatComponent />
      </ConnectionContextProvider>
    </AccountContextProvider>
  )
}

export default App;
