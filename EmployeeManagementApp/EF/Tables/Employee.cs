using System;
using System.Collections.Generic;

namespace EmployeeManagementApp.EF.Tables;

public partial class Employee
{
    public string? EmployeeName { get; set; }

    public int EmployeeId { get; set; }

    public decimal? PerformanceScore { get; set; }

    public decimal? LowestProjectRating { get; set; }

    public int? TotalProjectsCompleted { get; set; }
}
