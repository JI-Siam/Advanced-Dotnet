using Jasim_Plaza_Tenant_Portal.EF.Tables;

namespace Jasim_Plaza_Tenant_Portal.Data;

public static class DbSeeder
{
    public static void SeedDemoUsers(JasimPlazaDbContext db)
    {
        db.Database.EnsureCreated();

        var adminExists = db.Users.Any(u => u.Email == "admin@jasimplaza.local");
        if (!adminExists)
        {
            db.Users.Add(new User
            {
                Name = "Admin User",
                Email = "admin@jasimplaza.local",
                Password = "admin123",
                Role = "Admin"
            });
        }

        var ownerExists = db.Users.Any(u => u.Email == "owner@jasimplaza.local");
        if (!ownerExists)
        {
            db.Users.Add(new User
            {
                Name = "Shop Owner",
                Email = "owner@jasimplaza.local",
                Password = "owner123",
                Role = "Owner"
            });
        }

        var ownerTwoExists = db.Users.Any(u => u.Email == "owner2@jasimplaza.local");
        if (!ownerTwoExists)
        {
            db.Users.Add(new User
            {
                Name = "Owner Two",
                Email = "owner2@jasimplaza.local",
                Password = "owner123",
                Role = "Owner"
            });
        }

        if (!adminExists || !ownerExists || !ownerTwoExists)
        {
            db.SaveChanges();
        }
    }

    public static void EnsureCurrentMonthRentRecords(JasimPlazaDbContext db)
    {
        var now = DateTime.Today;
        var month = now.Month;
        var year = now.Year;

        var shops = db.Shops.ToList();

        foreach (var shop in shops)
        {
            var exists = db.RentRecords.Any(r => r.ShopId == shop.Id && r.Month == month && r.Year == year);
            if (exists)
            {
                continue;
            }

            db.RentRecords.Add(new RentRecord
            {
                ShopId = shop.Id,
                Month = month,
                Year = year,
                Amount = shop.MonthlyRent,
                Status = "Due",
                PaymentDate = null
            });
        }

        db.SaveChanges();
    }
}
