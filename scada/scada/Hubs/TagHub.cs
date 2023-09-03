using Microsoft.AspNetCore.SignalR;
using scada.Models;

namespace scada.Hubs
{
    public class TagHub : Hub
    {
        public async Task SendMessage(DITag tag)
        {
            await Clients.All.SendAsync("ReceiveMessage", tag);
        }
    }
}
