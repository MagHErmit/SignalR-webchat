import { RegistrationModel, LoginModel } from '../Contexts/AccountContext'


class AccountRepository {
    public login(user: LoginModel): Promise<Response> {
        return fetch('https://localhost:5001/account/login', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ name:user.name, password:user.password })
        })
    }

    public logout(): Promise<Response> {
        return fetch('https://localhost:5001/account/logout', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            }
        })
    }

    public registration(user: RegistrationModel): Promise<Response> {
        return fetch('https://localhost:5001/account/registration', {
            method: 'POST',
            credentials: "include",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({name: user.name, email:user.email, password:user.password})
        })
    }
}

const accountRepository = new AccountRepository()

export default accountRepository