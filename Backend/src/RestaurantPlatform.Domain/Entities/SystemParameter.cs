using System.ComponentModel.DataAnnotations;
using RestaurantPlatform.Domain.Common;

namespace RestaurantPlatform.Domain.Entities;

public class SystemParameter : BaseEntity
{
    [Key]
    [MaxLength(100)]
    public string ParamKey { get; set; } = string.Empty;

    [Required]
    public string ParamValue { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}