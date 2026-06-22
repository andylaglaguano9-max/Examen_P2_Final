using System;
using System.Collections.Generic;

namespace NorthwindApp.Models;

public partial class Territory
{
    public string TerritoryId { get; set; } = null!;

    public char TerritoryDescription { get; set; }

    public short RegionId { get; set; }

    public virtual Region Region { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
