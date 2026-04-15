using Rentt.Core.DTOs;
using Rentt.DataAccess;
using Rentt.Models;
using System.Threading.Tasks;

namespace Rentt.Business.Services // <-- İKİSİ DE AYNI ADRESTE
{
    public class CarService : ICarService
    {
        private readonly AppDbContext _context;

        public CarService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Car> AddCarAsync(AddCarDto carDto)
        {
            var car = new Car
            {
                Brand = carDto.Brand,
                Model = carDto.Model,
                Year = carDto.Year,
                DailyPrice = carDto.DailyPrice,
                ExtraKmFee = carDto.ExtraKmFee,
                CurrentMileage = carDto.CurrentMileage,
                MaintenanceMileageThreshold = carDto.MaintenanceMileageThreshold,
                IsAvailable = true // Yeni alınan araç her zaman müsaittir
            };

            // Aracıları baypas eden güvenli kayıt
            _context.Add(car);
            await _context.SaveChangesAsync();

            return car;
        }
    }
}