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
            await _hubContext.Clients.All.SendAsync("Notify",$"Arkadaşlar takım {teamCount} kişi olacaktır.").ConfigureAwait(false);
            return this.Ok();
        }
        //734bc0a64ae89563cb0fe175f48be8706539df82 => Sonarqube SignalRCore2 Token
    }
}
