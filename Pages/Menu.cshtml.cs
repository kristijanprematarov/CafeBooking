using CafeBooking.Web.Data;
using CafeBooking.Web.Models;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CafeBooking.Web.Pages;

public class MenuModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly TelemetryClient _telemetryClient;

    public MenuModel(ApplicationDbContext context,
        TelemetryClient telemetryClient)
    {
        _context = context;
        _telemetryClient = telemetryClient;
    }

    public List<MenuItem> MenuItems { get; set; } = new();

    public async Task OnGetAsync()
    {
        MenuItems = await _context.MenuItems
            .Where(m => m.IsAvailable)
            .OrderBy(m => m.Category)
            .ThenBy(m => m.Name)
            .ToListAsync();

        _telemetryClient.TrackEvent("MenuViewed", new Dictionary<string, string>
        {
            {"ItemCount", MenuItems.Count.ToString() }
        });

        _telemetryClient.TrackTrace($"Menu page accessed with {MenuItems.Count} items available.");
    }
}
