using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Jasim_Plaza_Tenant_Portal.EF.Tables;

public partial class RentRecord
{
    [Key]
    public int Id { get; set; }

    public int ShopId { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Amount { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public DateOnly? PaymentDate { get; set; }

    [ForeignKey("ShopId")]
    [InverseProperty("RentRecords")]
    public virtual Shop Shop { get; set; } = null!;
}
