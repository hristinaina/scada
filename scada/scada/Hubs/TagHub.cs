using Microsoft.AspNetCore.SignalR;
using scada.Models;

namespace scada.Hubs
{
    public class TagHub : Hub
    {
        public async Task SendTag(DITag tag)
        {
            await Clients.All.SendAsync("ReceiveMessage", tag);
        }
    }
}
