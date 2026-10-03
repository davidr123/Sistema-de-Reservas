using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantPlatform.Domain.Entities;

namespace RestaurantPlatform.Infrastructure.Persistence.Configurations;

public class AreaConfiguration : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        builder.ToTable("Areas");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(400);

        builder.Property(x => x.Capacity)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.FloorId,
            x.Name
        })
        .IsUnique();

        builder.HasOne(x => x.Floor)
            .WithMany(x => x.Areas)
            .HasForeignKey(x => x.FloorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}