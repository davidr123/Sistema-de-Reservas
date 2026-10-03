using System.ComponentModel.DataAnnotations;
using RestaurantPlatform.Domain.Common;

namespace RestaurantPlatform.Domain.Entities;

public class ContactMessage : BaseEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Phone { get; set; }

    [Required]
    public string Message { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "NEW";
}