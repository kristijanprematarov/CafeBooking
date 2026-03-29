using CafeBooking.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace CafeBooking.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly AppSettings _settings;

    public string SiteTitle => _settings.SiteTitle;
    public string ContactEmail => _settings.ContactEmail;

    public IndexModel(ILogger<IndexModel> logger,
        IOptions<AppSettings> settings)
    {
        _logger = logger;
        _settings = settings.Value;
    }

    public void OnGet()
    {

    }
}
