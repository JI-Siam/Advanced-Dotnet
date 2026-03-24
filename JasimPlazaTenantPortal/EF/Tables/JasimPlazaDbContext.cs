using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Jasim_Plaza_Tenant_Portal.EF.Tables;

public partial class JasimPlazaDbContext : DbContext
{
    public JasimPlazaDbContext()
    {
    }

    public JasimPlazaDbContext(DbContextOptions<JasimPlazaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<RentRecord> RentRecords { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=JasimPlazaDB;User Id=sa;Password=StrongPass123!;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RentRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RentReco__3214EC07310F091A");

            entity.HasOne(d => d.Shop).WithMany(p => p.RentRecords).HasConstraintName("FK_RentRecords_Shops");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shops__3214EC07E6E0E999");

            entity.HasOne(d => d.Owner).WithMany(p => p.Shops).HasConstraintName("FK_Shops_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0730D878DC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
