using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalRCore.CovidChart.API.Models;
using SignalRCore.CovidChart.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRCore.CovidChart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidChartController : ControllerBase
    {
        private readonly CovidService _covidService;

        public CovidChartController(CovidService covidService)
        {
            _covidService = covidService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid data)
        {
            await _covidService.SaveCovid(data).ConfigureAwait(false);
            //IQueryable<Covid> covidList = _covidService.GetList();
            return this.Ok(_covidService.GetCovidList());
        }

        [HttpGet]
        public IActionResult InitializeData()
        {
            Random rnd = new Random();
            Enumerable.Range(1, 10).ToList().ForEach(x =>
            {
                foreach (ECity item in Enum.GetValues(typeof(ECity)))
                {
                    var newCovid = new Covid()
                    {
                        City = item,
                        Count = rnd.Next(100, 1000),
                        CovidDate = DateTime.Now.AddDays(x)
                    };
                    _covidService.SaveCovid(newCovid).Wait();
                    Thread.Sleep(1000);
                }
            });
            return Ok("Covid19 dataları veritabanına kaydedildi.");
        }
    }
}
