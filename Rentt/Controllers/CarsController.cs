using Microsoft.AspNetCore.Mvc;
using Rentt.Business.Services; // <-- DOĞRU ADRES BURASI
using Rentt.Core.DTOs;
using System.Threading.Tasks;

namespace Rentt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCar([FromBody] AddCarDto addCarDto)
        {
            var createdCar = await _carService.AddCarAsync(addCarDto);

            return Ok(new
            {
                Message = "Araç filoya başarıyla eklendi!",
                Car = createdCar.Brand + " " + createdCar.Model,
                CarId = createdCar.Id
            });
        }
    }
}