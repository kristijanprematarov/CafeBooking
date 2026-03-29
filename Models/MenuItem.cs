using System.ComponentModel.DataAnnotations;

namespace CafeBooking.Web.Models;

public class MenuItem
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [Range(0.01, 10000)]
    public decimal Price { get; set; }

    public MenuCategory Category { get; set; }

    public bool IsAvailable { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum MenuCategory
{
    Coffee,
    Tea,
    Pastry,
    Sandwich,
    Salad,
    Dessert
}
