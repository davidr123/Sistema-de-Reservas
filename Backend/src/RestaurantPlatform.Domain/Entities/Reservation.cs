using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestaurantPlatform.Domain.Common;

namespace RestaurantPlatform.Domain.Entities;

public class Reservation : BaseEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string ReservationNumber { get; set; } = string.Empty;

    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;

    public DateTime StartAt { get; set; }

    public DateTime? EndAt { get; set; }

    public int AdultCount { get; set; } = 1;

    public int ChildCount { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int TotalPeople { get; private set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal PricePerAdult { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal PricePerChild { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal DiscountAmount { get; set; }

    [Column(TypeName = "decimal(12,2)")]
    public decimal TotalAmount { get; set; }

    [Required]
    [MaxLength(10)]
    public string Currency { get; set; } = "USD";

    [MaxLength(50)]
    public string? PromotionCode { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "PENDING";

    public int? CreatedByUserId { get; set; }

    public User? CreatedByUser { get; set; }

    public string? Notes { get; set; }

    public ICollection<ReservationArea> ReservationAreas { get; set; }
        = new List<ReservationArea>();

    public ICollection<Payment> Payments { get; set; }
        = new List<Payment>();

    public ICollection<Notification> Notifications { get; set; }
        = new List<Notification>();
}