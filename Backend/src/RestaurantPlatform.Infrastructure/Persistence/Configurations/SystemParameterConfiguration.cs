using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantPlatform.Domain.Entities;

namespace RestaurantPlatform.Infrastructure.Persistence.Configurations;

public class SystemParameterConfiguration : IEntityTypeConfiguration<SystemParameter>
{
    public void Configure(EntityTypeBuilder<SystemParameter> builder)
    {
        builder.ToTable("SystemParameters");

        builder.HasKey(x => x.ParamKey);

        builder.Property(x => x.ParamKey)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.ParamValue)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500);
    }
}