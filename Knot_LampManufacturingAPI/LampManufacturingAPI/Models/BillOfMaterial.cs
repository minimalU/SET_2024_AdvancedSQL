using System;
using System.Collections.Generic;

namespace LampManufacturingAPI.Models;

public partial class BillOfMaterial
{
    public int Bomid { get; set; }

    public int? ParentProductId { get; set; }

    public int? ChildProductId { get; set; }

    public int? BomQuantity { get; set; }

    public virtual Product? ChildProduct { get; set; }

    public virtual Product? ParentProduct { get; set; }
}
