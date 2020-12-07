class MessagesRepository {
    public getMessages(chatId: number): Promise<Response> {
        return fetch(`https://localhost:5001/messages/get?count=${20}&chatId=${chatId}`, {
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