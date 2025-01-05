using System;
using System.Collections.Generic;

namespace LampManufacturingAPI.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public bool? FinishedGoodsFlag { get; set; }

    public string? UnitMeasurementCode { get; set; }

    public int? InitialQty { get; set; }

    public virtual ICollection<BillOfMaterial> BillOfMaterialChildProducts { get; set; } = new List<BillOfMaterial>();

    public virtual ICollection<BillOfMaterial> BillOfMaterialParentProducts { get; set; } = new List<BillOfMaterial>();

    public virtual ICollection<Bin> Bins { get; set; } = new List<Bin>();

    public virtual ICollection<MaterialTransaction> MaterialTransactions { get; set; } = new List<MaterialTransaction>();

    public virtual ICollection<Tray> Trays { get; set; } = new List<Tray>();

    public virtual ICollection<WorkstationJob> WorkstationJobs { get; set; } = new List<WorkstationJob>();
}
