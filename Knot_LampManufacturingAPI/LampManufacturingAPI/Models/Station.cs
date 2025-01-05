using System;
using System.Collections.Generic;

namespace LampManufacturingAPI.Models;

public partial class Station
{
    public int StationId { get; set; }

    public string? StationName { get; set; }

    public virtual ICollection<Bin> Bins { get; set; } = new List<Bin>();

    public virtual ICollection<Tray> Trays { get; set; } = new List<Tray>();

    public virtual ICollection<WorkstationJob> WorkstationJobs { get; set; } = new List<WorkstationJob>();
}
