using Microsoft.AspNetCore.SignalR;
using scada.Models;

namespace scada.Hubs
{
    public class AlarmHub : Hub
    {
        public async Task SendAlarm(Alarm alarm)
        {
            await Clients.All.SendAsync("nekaPoruka", alarm);
        }
    }
}
