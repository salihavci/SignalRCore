using Microsoft.AspNetCore.SignalR;
using SignalRCore.CovidChart.API.Services;
using System.Threading.Tasks;

namespace SignalRCore.CovidChart.API.Hubs
{
    public class MyHub:Hub
    {
        private readonly CovidService _service;

        public MyHub(CovidService service)
        {
            _service = service;
        }

        public async Task GetCovidList()
        {
            await Clients.All.SendAsync("ReceiveCovidList", _service.GetCovidList());
        }
    }
}
