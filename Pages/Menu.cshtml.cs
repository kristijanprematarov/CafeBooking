using CafeBooking.Web.Data;
using CafeBooking.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CafeBooking.Web.Pages;

public class MenuModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public MenuModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<MenuItem> MenuItems { get; set; } = new();

    public async Task OnGetAsync()
    {
        MenuItems = await _context.MenuItems
            .Where(m => m.IsAvailable)
            .OrderBy(m => m.Category)
            .ThenBy(m => m.Name)
            .ToListAsync();
    }
}
