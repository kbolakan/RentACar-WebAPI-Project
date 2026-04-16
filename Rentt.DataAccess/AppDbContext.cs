using Microsoft.EntityFrameworkCore;
using Rentt.Models;

namespace Rentt.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<DamageRecord> DamageRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Hesaplama işlemleri için
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>().Property(c => c.DailyPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Car>().Property(c => c.ExtraKmFee).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Rental>().Property(r => r.BasePrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Rental>().Property(r => r.ExtraKmPrice).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Rental>().Property(r => r.LateFee).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Rental>().Property(r => r.TotalPrice).HasColumnType("decimal(18,2)");
        }
    }
}