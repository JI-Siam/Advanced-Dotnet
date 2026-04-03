using System;
using System.Collections.Generic;

namespace PractiseProject.EF.Tables;

public partial class Student
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal? Gpa { get; set; }
}
