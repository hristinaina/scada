using Microsoft.AspNetCore.SignalR;

namespace scada.WebSockets
{
    public class WebSocket : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
