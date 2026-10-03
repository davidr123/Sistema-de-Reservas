using System.ComponentModel.DataAnnotations;
using RestaurantPlatform.Domain.Common;

namespace RestaurantPlatform.Domain.Entities;

public class Payment : BaseEntity
{
    public int Id { get; set; }

    public int ReservationId { get; set; }

    public Reservation Reservation { get; set; } = null!;

    public decimal Amount { get; set; }

    [Required]
    [MaxLength(10)]
    public string Currency { get; set; } = "USD";

    [MaxLength(50)]
    public string? PaymentMethod { get; set; }

    [MaxLength(100)]
    public string? PaymentGateway { get; set; }

    [MaxLength(200)]
    public string? TransactionId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "PENDING";

    public DateTime? PaidAt { get; set; }
}