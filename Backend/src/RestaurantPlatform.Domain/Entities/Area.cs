using System.ComponentModel.DataAnnotations;
using RestaurantPlatform.Domain.Common;

namespace RestaurantPlatform.Domain.Entities;

public class Area : BaseEntity
{
    public int Id { get; set; }

    public int FloorId { get; set; }

    public Floor Floor { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(400)]
    public string? Description { get; set; }

    public int Capacity { get; set; }

    public ICollection<ReservationArea> ReservationAreas { get; set; }
        = new List<ReservationArea>();
}