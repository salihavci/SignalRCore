using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRCore.API.Hubs
{
    public class MyHub : Hub
    {
        public static List<string> Names { get; set; } = new List<string>();
        public static int OnlineCounter { get; set; } = 0;
        public async Task SendName(string name)
        {
            Names.Add(name);
            await Clients.All.SendAsync("ReceiveName", name).ConfigureAwait(false); //İlk parametre metot ismi, diğer parametreler argümanda gidecek datalar.
        }
        public async Task GetNames()
        {
            await Clients.All.SendAsync("ReceiveNames", Names).ConfigureAwait(false);
        }

        public override async Task OnConnectedAsync()
        {
            OnlineCounter++;
            await Clients.All.SendAsync("ReceiveOnlineCount", OnlineCounter).ConfigureAwait(false);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            OnlineCounter--;
            await Clients.All.SendAsync("ReceiveOnlineCount", OnlineCounter).ConfigureAwait(false);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
