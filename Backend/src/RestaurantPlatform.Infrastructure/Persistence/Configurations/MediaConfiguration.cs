using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantPlatform.Domain.Entities;

namespace RestaurantPlatform.Infrastructure.Persistence.Configurations;

public class MediaConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.ToTable("Media");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Url)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.MediaType)
            .HasMaxLength(50);

        builder.Property(x => x.RelatedEntity)
            .HasMaxLength(100);

        builder.Property(x => x.AltText)
            .HasMaxLength(250);

        builder.HasIndex(x => new
        {
            x.RelatedEntity,
            x.RelatedEntityId
        });
    }
}