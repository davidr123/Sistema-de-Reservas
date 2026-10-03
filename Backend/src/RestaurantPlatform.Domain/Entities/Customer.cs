using System.ComponentModel.DataAnnotations;
using RestaurantPlatform.Domain.Common;

namespace RestaurantPlatform.Domain.Entities;

public class Customer : BaseEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Email { get; set; }

    [MaxLength(50)]
    public string? Phone { get; set; }

    public string? Notes { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}