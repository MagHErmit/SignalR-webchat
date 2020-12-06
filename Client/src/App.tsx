import React from 'react'
import './App.css'
import Header from './Components/Header'
import { ChatComponent } from './Components/ChatComponent'
import { AccountContextProvider } from './Contexts/AccountContext'
import { LogingDialogContextProvder } from './Contexts/LoginDialogContext'
import { ConnectionContextProvider } from './Contexts/ConnectionContext'
import { RegistrationDialogContextProvider } from './Contexts/RegistrationDialogContext'
import { DialogComponent } from './Components/DialogListComponent'

function App() {
  return (
    <AccountContextProvider>
      <ConnectionContextProvider>
        <RegistrationDialogContextProvider>
          <LogingDialogContextProvder>
              <Header />
          </LogingDialogContextProvder>
        </RegistrationDialogContextProvider>
        <DialogComponent />
        <ChatComponent />
      </ConnectionContextProvider>
    </AccountContextProvider>
  )
}

export default App;
