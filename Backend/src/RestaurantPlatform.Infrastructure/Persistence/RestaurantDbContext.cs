using Microsoft.EntityFrameworkCore;
using RestaurantPlatform.Domain.Entities;

namespace RestaurantPlatform.Infrastructure.Persistence;

public class RestaurantDbContext : DbContext
{
    public RestaurantDbContext(
        DbContextOptions<RestaurantDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Floor> Floors => Set<Floor>();
    public DbSet<Area> Areas => Set<Area>();

    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<ReservationArea> ReservationAreas => Set<ReservationArea>();

    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Notification> Notifications => Set<Notification>();

    public DbSet<SpecialEvent> SpecialEvents => Set<SpecialEvent>();

    public DbSet<Media> Media => Set<Media>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<SystemParameter> SystemParameters => Set<SystemParameter>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(RestaurantDbContext).Assembly);
    }
}