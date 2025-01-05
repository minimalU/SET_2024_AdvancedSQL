using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Knot_AssemblyLine.Models
{
    public class WorkstationJob
    {
        public int JobId { get; set; }

        public int? ProductId { get; set; }

        public int? StationId { get; set; }

        public string ConfigurationId { get; set; }

        public string StationName { get; set; }

        public int? EmployeeId { get; set; }

        public int? TrayId { get; set; }

        public string BatchNumber { get; set; }
        public string OrderNumber { get; set; }

        public int? OrderQuantity { get; set; }

        public decimal? OrderAmount { get; set; }

        public int? YieldQuantity { get; set; }

        public int? QualityPassQuantity { get; set; }

        public int? QualityFailQuantity { get; set; }

        public DateTime? JobStartDatetime { get; set; }

        public DateTime? JobEndDatetime { get; set; }

        public string Status { get; set; }

    }
}
