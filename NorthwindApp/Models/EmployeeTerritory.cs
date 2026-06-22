using System;
using System.Collections.Generic;

namespace NorthwindApp.Models;

public partial class EmployeeTerritory
{
    public short EmployeeId { get; set; }

    public string TerritoryId { get; set; } = null!;
}
