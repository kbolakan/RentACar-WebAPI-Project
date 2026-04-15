using Microsoft.EntityFrameworkCore;
// YENİ KABLO: Modellerimiz artık Core binasının içinde!
using Rentt.Models;

// YENİ İKAMETGAH: Bu dosya artık koca bir DataAccess katmanı oldu!
namespace Rentt.DataAccess
{
    // Sınıfımızın Entity Framework yetenekleri kazanması için 'DbContext'ten miras alması şart.
    public class AppDbContext : DbContext
    {
        // Program.cs üzerinden göndereceğimiz veritabanı bağlantı ayarlarını içeri alıyoruz.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // İşte projenin ana taşıyıcı kolonları: C# sınıflarımızı SQL tablolarına dönüştüren DbSet'ler.
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<DamageRecord> DamageRecords { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }

        // İddialı bir mimaride tablolar arası ilişkileri ve kısıtlamaları (Fluent API) 
        // burada şekillendirebiliriz.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Örneğin, Email adreslerinin sistemde tekil (Unique) olmasını sağlayan kısıtlama:
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}