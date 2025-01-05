using System;
using System.Collections.Generic;

namespace LampManufacturingAPI.Models;

public partial class Bin
{
    public int BinId { get; set; }

    public int? ProductId { get; set; }

    public int? StationId { get; set; }

    public int? PartQuantity { get; set; }

    public int? ReplenishentQty { get; set; }

    public int? ThresholdQty { get; set; }

    public bool? IsReplenishmentNeeded { get; set; }

    public DateTime? LastRepTime { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Station? Station { get; set; }
}
