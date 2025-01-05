using System;
using System.Collections.Generic;

namespace LampManufacturingAPI.Models;

public partial class Inventory
{
    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public bool? QualityPass { get; set; }

    public string? BatchNumber { get; set; }

    public virtual Product Product { get; set; } = null!;
}
