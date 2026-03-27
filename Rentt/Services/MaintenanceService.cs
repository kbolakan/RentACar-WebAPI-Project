using Microsoft.EntityFrameworkCore;
using Rentt.Data;
using Rentt.Models;

namespace Rentt.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly AppDbContext _context;

        public MaintenanceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> SendToMaintenanceAsync(int carId, string description)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null) throw new Exception("Sistemde böyle bir araç bulunamadı.");
            if (!car.IsAvailable) throw new Exception("Araç şu anda müsait değil (Kirada veya zaten bakımda).");

            var maintenance = new Maintenance
            {
                CarId = carId,
                Description = description,
                StartDate = DateTime.UtcNow
                // Kırmızı yanan IsCompleted satırını tamamen sildik!
            };

            car.IsAvailable = false; // Aracı kiralanamaz yap

            _context.Maintenances.Add(maintenance);
            await _context.SaveChangesAsync();

            return new { Message = "Araç başarıyla bakıma alındı.", MaintenanceId = maintenance.Id, Car = car.Brand + " " + car.Model };
        }

        public async Task<object> ReturnFromMaintenanceAsync(int maintenanceId)
        {
            var maintenance = await _context.Maintenances.Include(m => m.Car).FirstOrDefaultAsync(m => m.Id == maintenanceId);
            if (maintenance == null) throw new Exception("Bakım kaydı bulunamadı.");

            // IsCompleted yerine EndDate'i kontrol ediyoruz! (Bitiş tarihi varsa zaten bitmiştir)
            if (maintenance.EndDate != null) throw new Exception("Bu bakım zaten tamamlanmış.");

            maintenance.EndDate = DateTime.UtcNow; // Bakımı şu anki saatle bitir
            maintenance.Car.IsAvailable = true; // Aracı tekrar kiralik yap

            await _context.SaveChangesAsync();

            return new { Message = "Araç bakımdan çıktı ve tekrar kiralanmaya hazır.", MaintenanceId = maintenance.Id, Car = maintenance.Car.Brand + " " + maintenance.Car.Model };
        }
    }
}