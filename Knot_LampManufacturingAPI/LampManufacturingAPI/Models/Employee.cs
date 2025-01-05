using System;
using System.Collections.Generic;

namespace LampManufacturingAPI.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? Name { get; set; }

    public float? SkillLevel { get; set; }

    public virtual ICollection<WorkstationJob> WorkstationJobs { get; set; } = new List<WorkstationJob>();
}
