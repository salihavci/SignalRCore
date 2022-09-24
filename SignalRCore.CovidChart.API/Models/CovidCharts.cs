using System.Collections.Generic;

namespace SignalRCore.CovidChart.API.Models
{
    public class CovidCharts
    {
        public CovidCharts()
        {
            Counts = new List<int>();
        }
        public string CovidDate { get; set; }
        public List<int> Counts { get; set; }
    }
}
