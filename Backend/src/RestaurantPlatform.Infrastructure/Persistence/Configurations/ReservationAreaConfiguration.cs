using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantPlatform.Domain.Entities;

namespace RestaurantPlatform.Infrastructure.Persistence.Configurations;

public class ReservationAreaConfiguration : IEntityTypeConfiguration<ReservationArea>
{
    public void Configure(EntityTypeBuilder<ReservationArea> builder)
    {
        builder.ToTable("ReservationAreas");

        builder.HasKey(x => new
        {
            x.ReservationId,
            x.AreaId
        });

        builder.Property(x => x.Notes);

        builder.HasOne(x => x.Reservation)
            .WithMany(x => x.ReservationAreas)
            .HasForeignKey(x => x.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Area)
            .WithMany(x => x.ReservationAreas)
            .HasForeignKey(x => x.AreaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}