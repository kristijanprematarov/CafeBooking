using CafeBooking.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeBooking.Web.Data;

public class DbSeeder
{
    private readonly ApplicationDbContext _context;

    public DbSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.MenuItems.AnyAsync())
            return; // Already seeded

        var menuItems = new List<MenuItem>
        {
            new() { Name = "Espresso", Description = "Rich Italian espresso", Price = 3.50m, Category = MenuCategory.Coffee },
            new() { Name = "Cappuccino", Description = "Espresso with steamed milk", Price = 4.50m, Category = MenuCategory.Coffee },
            new() { Name = "Latte", Description = "Smooth espresso with steamed milk", Price = 4.75m, Category = MenuCategory.Coffee },
            new() { Name = "Croissant", Description = "Buttery French pastry", Price = 3.50m, Category = MenuCategory.Pastry },
            new() { Name = "Chocolate Muffin", Description = "Rich chocolate muffin", Price = 4.00m, Category = MenuCategory.Pastry },
            new() { Name = "Turkey Club Sandwich", Description = "Turkey, bacon, lettuce, tomato", Price = 8.50m, Category = MenuCategory.Sandwich },
        };

        await _context.MenuItems.AddRangeAsync(menuItems);
        await _context.SaveChangesAsync();
    }
}
