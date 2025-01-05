using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knot_AssemblyLine.Models
{
    public class ChartData
    {
        public int? StationId { get; set; }
        public string ConfigurationId { get; set; }
        public string StationName { get; set; }
        public string BatchNumber { get; set; }
        public string OrderNumber { get; set; }
        public int? OrderQuantity { get; set; }
        public decimal? OrderAmount { get; set; }
        public int? YieldQuantity { get; set; }
        public int? QualityPassQuantity { get; set; }
        public int? QualityFailQuantity { get; set; }
    }
}
