using Microsoft.AspNetCore.SignalR;
using scada.Models;

namespace scada.WebSockets
{
    public class WebSocket : Hub
    {
        public async Task SendMessage(DITag tag)
        {
            await Clients.All.SendAsync("ReceiveMessage", tag);
        }
    }
}
