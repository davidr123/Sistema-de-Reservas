using System.ComponentModel.DataAnnotations;
using RestaurantPlatform.Domain.Common;

namespace RestaurantPlatform.Domain.Entities;

public class Floor : BaseEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(400)]
    public string? Description { get; set; }

    public ICollection<Area> Areas { get; set; } = new List<Area>();
}