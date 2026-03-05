
using Microsoft.AspNetCore.Mvc;

public class ProductController : Controller
{
    public IActionResult List()
    {
        var data = new List<Product>();
        Random rand = new Random();
        for (int i = 1; i <= 10; i++)
        {
            data.Add(new Product()
            {
                Id = i,
                Name = "Product " + i,
                Quantity = (int)rand.NextInt64(1, 20),
                Price = (float)rand.NextDouble() * 2000
            });
        }
        return View(data);
    }

    public IActionResult Details(int id)
    {
        Random rand = new Random();
        var data = new Product()
        {
            Id = id,
            Name = "Product " + id,
            Quantity = (int)rand.NextInt64(1, 20),
            Price = (float)rand.NextDouble() * 2000
        };

        return View(data);
    }


    public IActionResult Create()
    {
        return View();
    }
}