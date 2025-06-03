using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RestaurantReservation.API.Entities;
using System.Diagnostics.Metrics;

namespace RestaurantReservation.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Table> Tables => Set<Table>(); // Таблиця столиків
        public DbSet<Booking> Bookings => Set<Booking>(); // Таблиця бронювань
        public DbSet<User> Users => Set<User>(); // Таблиця користувачів

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Table>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Booking>(b =>
            {
                b.HasOne(b => b.Table)
                    .WithMany(t => t.Bookings)
                    .HasForeignKey(b => b.TableId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
                );
        }
    }
}