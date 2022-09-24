using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRCore.CovidChart.API.Hubs;
using SignalRCore.CovidChart.API.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCore.CovidChart.API.Services
{
    public class CovidService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<MyHub> _hubContext;
        public CovidService(AppDbContext context, IHubContext<MyHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IQueryable<Covid> GetList()
        {
            return _context.Covid.AsQueryable();
        }

        public async Task SaveCovid(Covid data)
        {
            await _context.Covid.AddAsync(data).ConfigureAwait(false);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveCovidList", GetCovidList());
        }

        public List<CovidCharts> GetCovidList()
        {
            List<CovidCharts> charts = new List<CovidCharts>();
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                var commandText = @"select tarih,[1],[2],[3],[4],[5]
                    from (select [City],[Count],Cast([CovidDate] as date) as tarih from Covid) as CovidTable
                    pivot(SUM([Count]) for City IN ([1],[2],[3],[4],[5])) as PivotTable
                    order by tarih asc";
                command.CommandText = commandText;
                command.CommandType = CommandType.Text;
                _context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CovidCharts data = new CovidCharts();
                        data.CovidDate = reader.GetDateTime(0).ToShortDateString(); //Sorgudaki 0. indexi datetime olarak al ve stringe çevir dedik.
                        Enumerable.Range(1, 5).ToList().ForEach(x =>
                        {
                            if (System.DBNull.Value.Equals(reader[x]))
                            {
                                data.Counts.Add(0);
                            }
                            else
                            {
                                data.Counts.Add(reader.GetInt32(x));
                            }
                        });
                        charts.Add(data);
                    }
                }
                _context.Database.CloseConnection();
            }
            return charts;
        }
    }
}
