using System.ComponentModel.DataAnnotations;
using RestaurantPlatform.Domain.Common;

namespace RestaurantPlatform.Domain.Entities;

public class Media : BaseEntity
{
    public int Id { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Url { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? MediaType { get; set; }

    [MaxLength(100)]
    public string? RelatedEntity { get; set; }

    public int? RelatedEntityId { get; set; }

    [MaxLength(250)]
    public string? AltText { get; set; }

    public int? SortOrder { get; set; }

    public bool IsPublic { get; set; } = true;
}