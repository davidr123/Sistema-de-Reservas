using System.ComponentModel.DataAnnotations;
using RestaurantPlatform.Domain.Common;

namespace RestaurantPlatform.Domain.Entities;

public class Notification : BaseEntity
{
    public int Id { get; set; }

    public int? ReservationId { get; set; }

    public Reservation? Reservation { get; set; }

    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Channel { get; set; } = string.Empty;

    public string? Payload { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "PENDING";

    public int AttemptCount { get; set; }

    public DateTime? LastAttemptAt { get; set; }

    public DateTime? SentAt { get; set; }
}