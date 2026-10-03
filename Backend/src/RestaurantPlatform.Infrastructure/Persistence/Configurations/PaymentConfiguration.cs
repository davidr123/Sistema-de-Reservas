using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantPlatform.Domain.Entities;

namespace RestaurantPlatform.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .HasPrecision(12, 2)
            .IsRequired();

        builder.Property(x => x.Currency)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(x => x.PaymentMethod)
            .HasMaxLength(50);

        builder.Property(x => x.PaymentGateway)
            .HasMaxLength(100);

        builder.Property(x => x.TransactionId)
            .HasMaxLength(200);

        builder.Property(x => x.Status)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.TransactionId);

        builder.HasOne(x => x.Reservation)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}