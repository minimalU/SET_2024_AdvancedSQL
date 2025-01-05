// https://learn.microsoft.com/en-us/ef/core/modeling/keyless-entity-types?tabs=data-annotations
using Microsoft.EntityFrameworkCore;

namespace LampManufacturingAPI.Models
{
    [Keyless]
    public class BinsWithWorkstationjob
    {
        public int BinId { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? StationId { get; set; }
        public int? PartQuantity { get; set; }
        public int? ReplenishentQty { get; set; }
        public int? ThresholdQty { get; set; }
        public bool? IsReplenishmentNeeded { get; set; }
        public int JobId { get; set; }

        public int? FG_Id { get; set; }
        public string? ConfigurationId { get; set; }
        public string? StationName { get; set; }
        public int? EmployeeId { get; set; }
        public int? TrayId { get; set; }
        public string? BatchNumber { get; set; }
        public string? OrderNumber { get; set; }
        public int? OrderQuantity { get; set; }
        public decimal? OrderAmount { get; set; }
        public int? YieldQuantity { get; set; }
        public int? QualityPassQuantity { get; set; }
        public int? QualityFailQuantity { get; set; }
        public DateTime? JobStartDatetime { get; set; }
        public DateTime? JobEndDatetime { get; set; }
        public string? Status { get; set; }
    }
}
