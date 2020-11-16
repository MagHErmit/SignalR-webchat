import * as SignalR from '@microsoft/signalr'

class SignalRManager {
    public static instance: SignalRManager = new SignalRManager()

    public connection: SignalR.HubConnection;

    constructor() {
        this.connection = new SignalR.HubConnectionBuilder()
            .withUrl('https://localhost:5001/lothub', {
                transport: SignalR.HttpTransportType.WebSockets,
                skipNegotiation: true
            })
            .withAutomaticReconnect({
                nextRetryDelayInMilliseconds: retryContext => {
                    return 5000;
                }
            })
            .build()
    }

    public async start() {
        if (this.connection.state === SignalR.HubConnectionState.Disconnected)
        {
            await this.connection.start()
        }
    }

    public async stop() {
        await this.connection.stop()
    }

    public isDisconnected() {
        return this.connection.state === SignalR.HubConnectionState.Disconnected
    }
}

export default SignalRManager