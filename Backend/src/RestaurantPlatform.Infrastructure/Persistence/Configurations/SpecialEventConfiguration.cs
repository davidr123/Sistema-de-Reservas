using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantPlatform.Domain.Entities;

namespace RestaurantPlatform.Infrastructure.Persistence.Configurations;

public class SpecialEventConfiguration : IEntityTypeConfiguration<SpecialEvent>
{
    public void Configure(EntityTypeBuilder<SpecialEvent> builder)
    {
        builder.ToTable("SpecialEvents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description);

        builder.HasIndex(x => new
        {
            x.StartAt,
            x.EndAt
        });

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}