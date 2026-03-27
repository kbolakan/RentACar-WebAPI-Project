using Microsoft.AspNetCore.Mvc;
using Rentt.Services;

namespace Rentt.Controllers
{
    // API'mizin dış dünyaya açılan adresi: http://localhost:port/api/Rentals
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        // Dependency Injection: Controller'a "Sen aptalsın, hesaplama yapma, işi Service'e devret" diyoruz.
        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        // 1. VİTRİN: ARAÇ KİRALAMA EKRANI
        [HttpPost("rent")]
        public async Task<IActionResult> RentCar([FromBody] RentRequest request)
        {
            try
            {
                // Müşteriden gelen veriyi alıp doğrudan beynimize (Service) yolluyoruz
                var result = await _rentalService.RentCarAsync(request.UserId, request.CarId, request.StartDate, request.EndDate);
                return Ok(result); // İşlem başarılıysa 200 OK ve faturayı döndür
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Hata varsa (örn: Araç müsait değilse) 400 Bad Request döndür
            }
        }

        // 2. VİTRİN: ARACI İADE ETME EKRANI
        [HttpPost("return")]
        public async Task<IActionResult> ReturnCar([FromBody] ReturnRequest request)
        {
            try
            {
                var result = await _rentalService.ReturnCarAsync(request.RentalId, request.ReturnMileage, request.ReturnDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    // --- DIŞARIDAN GELECEK VERİ PAKETLERİNİN (DTO) TASLAKLARI ---
    // Swagger ekranında düzgün formlar çıkması için bu küçük sınıfları kullanıyoruz.
    public class RentRequest
    {
        public int UserId { get; set; }
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class ReturnRequest
    {
        public int RentalId { get; set; }
        public int ReturnMileage { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}