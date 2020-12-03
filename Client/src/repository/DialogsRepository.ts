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
}

const dialogsRepository = new DialogsRepository()

export default dialogsRepository