using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rentt.Business.Abstract; // <-- SİLİNEN KİLİT KABLO GERİ EKLENDİ
using Rentt.DataAccess;
using Rentt.Models;

namespace Rentt.Business.Services
{
    // <-- SADECE IMaintenanceService OLARAK DÜZELTİLDİ
    public class MaintenanceService : IMaintenanceService
    {
        private readonly AppDbContext _context;

        public MaintenanceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> SendToMaintenanceAsync(int carId, string description)
        {
            var car = await _context.FindAsync<Car>(carId);

            if (car == null) throw new Exception("Sistemde böyle bir araç bulunamadı.");
            if (!car.IsAvailable) throw new Exception("Araç şu anda müsait değil (Kirada veya zaten bakımda).");

            var maintenance = new Maintenance
            {
                CarId = carId,
                Description = description,
                StartDate = DateTime.UtcNow
            };

            car.IsAvailable = false;

            _context.Add(maintenance);
            await _context.SaveChangesAsync();

            return new { Message = "Araç başarıyla bakıma alındı.", MaintenanceId = maintenance.Id };
        }

        public async Task<object> ReturnFromMaintenanceAsync(int maintenanceId)
        {
            var maintenance = await _context.Set<Maintenance>()
                .Include(m => m.Car)
                .FirstOrDefaultAsync(m => m.Id == maintenanceId);

            if (maintenance == null) throw new Exception("Bakım kaydı bulunamadı.");

            if (maintenance.EndDate != null) throw new Exception("Bu bakım zaten tamamlanmış.");

            maintenance.EndDate = DateTime.UtcNow;
            maintenance.Car.IsAvailable = true;

            await _context.SaveChangesAsync();

            return new { Message = "Araç bakımdan çıktı ve tekrar kiralanmaya hazır.", MaintenanceId = maintenance.Id };
        }
    }
}