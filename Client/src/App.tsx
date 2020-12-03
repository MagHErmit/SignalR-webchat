import React from 'react'
import './App.css'
import Header from './Components/Header'
import { ChatComponent } from './Components/ChatComponent'
import { AccountContextProvider } from './Contexts/AccountContext'
import { LogingDialogContextProvder } from './Contexts/LoginDialogContext'
import { ConnectionContextProvider } from './Contexts/ConnectionContext'
import { RegistrationDialogContextProvider } from './Contexts/RegistrationDialogContext'
import { DialogListComponent } from './Components/DialogListComponent'

function App() {
  return (
    <AccountContextProvider>
      <ConnectionContextProvider>
        <RegistrationDialogContextProvider>
          <LogingDialogContextProvder>
              <Header />
          </LogingDialogContextProvder>
        </RegistrationDialogContextProvider>
        <DialogListComponent />
        <ChatComponent />
      </ConnectionContextProvider>
    </AccountContextProvider>
  )
}

export default App;
