using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Jasim_Plaza_Tenant_Portal.EF.Tables;

public partial class Shop
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string ShopName { get; set; } = null!;

    [StringLength(150)]
    public string? Location { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal MonthlyRent { get; set; }

    public int OwnerId { get; set; }

    [ForeignKey("OwnerId")]
    [InverseProperty("Shops")]
    public virtual User Owner { get; set; } = null!;

    [InverseProperty("Shop")]
    public virtual ICollection<RentRecord> RentRecords { get; set; } = new List<RentRecord>();
}
