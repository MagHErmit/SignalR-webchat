class MessagesRepository {
    public getMessages(): Promise<Response> {
        return fetch('https://localhost:5001/messages/get', {
            method: 'GET',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            }
        })
    }
}

const messagesRepository = new MessagesRepository()

export default messagesRepository