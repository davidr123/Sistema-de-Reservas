using System.ComponentModel.DataAnnotations;
using RestaurantPlatform.Domain.Common;

namespace RestaurantPlatform.Domain.Entities;

public class SpecialEvent : BaseEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public bool AllDay { get; set; }

    public bool BlocksAvailability { get; set; } = true;

    public int? CapacityBlocked { get; set; }

    public bool IsPrivate { get; set; }

    public int? CreatedByUserId { get; set; }

    public User? CreatedByUser { get; set; }
}