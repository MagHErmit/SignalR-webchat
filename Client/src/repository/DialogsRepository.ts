class DialogsRepository {
    public getDialogs(): Promise<Response> {
        return fetch('https://localhost:5001/dialogs/get', {
            method: 'GET',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            }
        })
    }

    public createDialog(name: string): Promise<Response> {
        return fetch(`https://localhost:5001/dialogs/create?name=${name}`, {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-type': 'application/json'
            }
        })
    }

    public addUserToDialog(userName: string, chatId: number): Promise<Response> {
        return fetch(`https://localhost:5001/dialogs/addUserToChat?userName=${userName}&chatId=${chatId}`, {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-type' : 'application/json'
            }
        })
    }
}

const dialogsRepository = new DialogsRepository()

export default dialogsRepository