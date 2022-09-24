using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRCore.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCore.API.Hubs
{
    public class MyHub : Hub
    {
        private readonly AppDbContext _context;

        public MyHub(AppDbContext context)
        {
            _context = context;
        }

        public static List<string> Names { get; set; } = new List<string>();
        public static int OnlineCounter { get; set; } = 0;
        public static int TeamCounter { get; set; } = 7;
        public async Task SendName(string name)
        {
            if (Names.Count >= TeamCounter)
            {
                await Clients.Caller.SendAsync("Error", $"Takım en fazla {TeamCounter} kişi olabilir.");
            }
            else
            {
                Names.Add(name);
                await Clients.All.SendAsync("ReceiveName", name).ConfigureAwait(false); //İlk parametre metot ismi, diğer parametreler argümanda gidecek datalar.
            }
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

        public async Task AddToGroup(string teamName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, teamName).ConfigureAwait(false);
        }

        public async Task RemoveToGroup(string teamName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, teamName).ConfigureAwait(false);
        }

        public async Task SendNameByGroup(string name, string teamName)
        {
            var team = _context.Team.Where(x => x.Name == teamName).FirstOrDefault();
            if (team != null)
            {
                team.User.Add(new User() { Name = name });
            }
            else
            {
                var newTeam = new Team() { Name = teamName };
                newTeam.User.Add(new User() { Name = name });
                _context.Team.Add(newTeam);
            }
            await _context.SaveChangesAsync();
            await Clients.Group(teamName).SendAsync("ReceiveMessageByGroup", name, team.Id);
        }

        public async Task GetNamesByGroup()
        {
            var team = _context.Team.Include(x => x.User).Select(m => new
            {
                teamId = m.Id,
                users = m.User.ToList()
            });
            await Clients.All.SendAsync("ReceiveNamesByGroup", team);
        }

        public async Task SendProduct(Product p)
        {
            await Clients.All.SendAsync("ReceiveProducts", p).ConfigureAwait(false);
        }
    }
}
