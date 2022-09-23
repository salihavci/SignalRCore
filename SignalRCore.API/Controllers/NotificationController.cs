using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRCore.API.Hubs;
using System.Threading.Tasks;

namespace SignalRCore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<MyHub> _hubContext;

        public NotificationController(IHubContext<MyHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("{teamCount}")]
        public async Task<IActionResult> SetTeamCount(int teamCount)
        {
            MyHub.TeamCounter = teamCount;
            await _hubContext.Clients.All.SendAsync("Notify",$"Arkadaşlar takım {teamCount} kişi olacaktır.").ConfigureAwait(false);
            return this.Ok();
        }
        //762acfb7c0b4c5e6c9aef435af261fb5869e82c1 => Sonarqube SignalRCore2 Token
    }
}
