using System;
using System.Collections.Generic;

namespace LampManufacturingAPI.Models;

public partial class Tray
{
    public int TrayId { get; set; }

    public int? ProductId { get; set; }

    public int? StationId { get; set; }

    public int? ProductQuantity { get; set; }

    public int? TrayMaxQuantity { get; set; }

    public string? BatchNumber { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Station? Station { get; set; }

    public virtual ICollection<WorkstationJob> WorkstationJobs { get; set; } = new List<WorkstationJob>();
}
