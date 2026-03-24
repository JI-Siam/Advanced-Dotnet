using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Jasim_Plaza_Tenant_Portal.EF.Tables;

[Index("Email", Name = "UQ__Users__A9D10534D8E4C009", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(100)]
    public string Password { get; set; } = null!;

    [StringLength(20)]
    public string Role { get; set; } = null!;

    [InverseProperty("Owner")]
    public virtual ICollection<Shop> Shops { get; set; } = new List<Shop>();
}
