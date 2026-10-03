using System.ComponentModel.DataAnnotations;
using RestaurantPlatform.Domain.Common;

namespace RestaurantPlatform.Domain.Entities;

public class AuditLog : BaseEntity
{
    public long Id { get; set; }

    public int? UserId { get; set; }

    public User? User { get; set; }

    [Required]
    [MaxLength(100)]
    public string Action { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Entity { get; set; }

    [MaxLength(100)]
    public string? EntityId { get; set; }

    public string? Details { get; set; }
}