using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRCore.API.Hubs
{
    public class MyHub: Hub
    {
        public static List<string> Names { get; set; } = new List<string>();
        public async Task SendName(string name)
        {
            Names.Add(name);
            await Clients.All.SendAsync("ReceiveName",name).ConfigureAwait(false); //İlk parametre metot ismi, diğer parametreler argümanda gidecek datalar.
        }
        public async Task GetNames()
        {
            await Clients.All.SendAsync("ReceiveNames",Names).ConfigureAwait(false);
        }
    }
}
