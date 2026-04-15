using Rentt.Models;

namespace Rentt.Services
{
    public interface IRentalService
    {
        Task<Rental> RentCarAsync(int userId, int carId, DateTime startDate, DateTime endDate);

        Task<Rental> ReturnCarAsync(int rentalId, int returnMileage, DateTime returnDate);
    }
}