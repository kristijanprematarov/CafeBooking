namespace CafeBooking.Web.Models;

public class AppSettings
{
    public const string SectionName = "AppSettings";

    public string SiteTitle { get; set; } = "Cafe Booking";
    public string ContactEmail { get; set; } = "info@cafebooking.com";
    public int MaxBookingsPerDay { get; set; } = 50;
    public bool EnableOnlinePayments { get; set; } = false;
}