using System;
using System.Collections.Generic;

namespace LampManufacturingAPI.Models;

public partial class MaterialTransaction
{
    public int TransactionId { get; set; }

    public int? ProductId { get; set; }

    public string? MovementType { get; set; }

    public string? BatchNumber { get; set; }

    public DateTime? Timestamp { get; set; }

    public int? Quantity { get; set; }

    public virtual Product? Product { get; set; }
}
