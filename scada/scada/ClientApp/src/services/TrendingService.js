import { HubConnectionBuilder } from "@microsoft/signalr";

class TrendingService {

    createConnection(url) {
        const connection = new HubConnectionBuilder()
            .withUrl(url)
            .build();

        connection
            .start()
            .then(() => {
                console.log("Connected to WebSocket server");
            })
            .catch((error) => {
                console.error(error);
            });

        return connection
    }
}

export default new TrendingService();

