using System.ComponentModel.DataAnnotations;

namespace CafeBooking.Web.Models;

public class Booking
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string? PhoneNumber { get; set; }

    [Required]
    public DateTime BookingDate { get; set; }

    [Required]
    public TimeSpan BookingTime { get; set; }

    [Range(1, 20)]
    public int NumberOfGuests { get; set; }

    [StringLength(500)]
    public string? SpecialRequests { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public BookingStatus Status { get; set; } = BookingStatus.Pending;
}

public enum BookingStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed
}
