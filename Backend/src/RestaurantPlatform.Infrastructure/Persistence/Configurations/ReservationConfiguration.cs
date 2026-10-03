using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantPlatform.Domain.Entities;

namespace RestaurantPlatform.Infrastructure.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ReservationNumber)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(x => x.ReservationNumber)
            .IsUnique();

        builder.HasOne(x => x.Customer)
            .WithMany(x => x.Reservations)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.TotalPeople)
            .HasComputedColumnSql("[AdultCount] + [ChildCount]");

        builder.Property(x => x.PricePerAdult)
            .HasPrecision(12, 2);

        builder.Property(x => x.PricePerChild)
            .HasPrecision(12, 2);

        builder.Property(x => x.DiscountAmount)
            .HasPrecision(12, 2);

        builder.Property(x => x.TotalAmount)
            .HasPrecision(12, 2);

        builder.Property(x => x.Currency)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.PromotionCode)
            .HasMaxLength(50);

        builder.Property(x => x.Status)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.StartAt);

        builder.HasIndex(x => x.Status);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}