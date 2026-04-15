using Microsoft.EntityFrameworkCore;
using Rentt.Business.Abstract;
using Rentt.DataAccess;
using Rentt.Models;
using Rentt.Services;
using System;
using System.Threading.Tasks;

namespace Rentt.Business.Services
{
    public class RentalService : IRentalService
    {
        private readonly AppDbContext _context;

        public RentalService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Rental> RentCarAsync(int userId, int carId, DateTime startDate, DateTime endDate)
        {
            var car = await _context.FindAsync<Car>(carId);
            if (car == null) throw new Exception("Sistemde böyle bir araç bulunamadı.");
            if (!car.IsAvailable) throw new Exception("Üzgünüz, bu araç şu anda başkası tarafından kiralanmış veya bakımda.");

            var user = await _context.FindAsync<User>(userId);
            if (user == null) throw new Exception("Kullanıcı bulunamadı.");

            if (startDate < DateTime.UtcNow.Date || endDate <= startDate)
                throw new Exception("Geçersiz tarih aralığı seçtiniz.");

            var rental = new Rental
            {
                UserId = userId,
                CarId = carId,
                StartDate = startDate,
                EndDate = endDate,
                StartMileage = car.CurrentMileage,
                Status = "Active"
            };

            car.IsAvailable = false;

            _context.Add(rental);
            await _context.SaveChangesAsync();

            return rental;
        }

        public async Task<Rental> ReturnCarAsync(int rentalId, int returnMileage, DateTime returnDate)
        {
            var rental = await _context.Set<Rental>()
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == rentalId);

            if (rental == null) throw new Exception("Böyle bir kiralama kaydı bulunamadı.");
            if (rental.Status != "Active") throw new Exception("Bu araç zaten iade edilmiş, işlem yapılamaz.");
            if (returnMileage < rental.StartMileage) throw new Exception("İade kilometresi, başlangıç kilometresinden küçük olamaz! Sayaçla oynanmış olabilir.");

            var rentDuration = (rental.EndDate - rental.StartDate).TotalDays;
            var actualRentDays = Math.Max(1, (int)Math.Ceiling(rentDuration));
            rental.BasePrice = actualRentDays * rental.Car.DailyPrice;

            int allowedMileage = actualRentDays * 200;
            int totalMileageDriven = returnMileage - rental.StartMileage;

            if (totalMileageDriven > allowedMileage)
            {
                int extraKm = totalMileageDriven - allowedMileage;
                rental.ExtraKmPrice = extraKm * rental.Car.ExtraKmFee;
            }

            if (returnDate > rental.EndDate)
            {
                var lateDuration = (returnDate - rental.EndDate).TotalDays;
                var lateDays = (int)Math.Ceiling(lateDuration);
                rental.LateFee = lateDays * (rental.Car.DailyPrice * 1.5m);
            }

            rental.TotalPrice = rental.BasePrice + rental.ExtraKmPrice + rental.LateFee;
            rental.ReturnDate = returnDate;
            rental.ReturnMileage = returnMileage;
            rental.Status = "Completed";

            rental.Car.CurrentMileage = returnMileage;

            if (rental.Car.CurrentMileage >= rental.Car.MaintenanceMileageThreshold)
            {
                rental.Car.IsAvailable = false;
            }
            else
            {
                rental.Car.IsAvailable = true;
            }

            await _context.SaveChangesAsync();

            // Eksik olan o kritik dönüş satırı ve kapanış parantezleri eklendi!
            return rental;
        }
    }
}