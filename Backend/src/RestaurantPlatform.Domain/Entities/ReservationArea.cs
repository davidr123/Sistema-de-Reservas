using RestaurantPlatform.Domain.Common;

namespace RestaurantPlatform.Domain.Entities;

public class ReservationArea : BaseEntity
{
    public int ReservationId { get; set; }

    public Reservation Reservation { get; set; } = null!;

    public int AreaId { get; set; }

    public Area Area { get; set; } = null!;

    public string? Notes { get; set; }
}