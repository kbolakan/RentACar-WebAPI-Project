using Microsoft.AspNetCore.Mvc;
using Rentt.Services;

namespace Rentt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DamageRecordsController : ControllerBase
    {
        private readonly IDamageRecordService _damageRecordService;

        public DamageRecordsController(IDamageRecordService damageRecordService)
        {
            _damageRecordService = damageRecordService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRecord([FromBody] CreateDamageRequest request)
        {
            try
            {
                var result = await _damageRecordService.CreateDamageRecordAsync(request.CarId, request.Description);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("car/{carId}")]
        public async Task<IActionResult> GetRecordsForCar(int carId)
        {
            try
            {
                var result = await _damageRecordService.GetDamageRecordsByCarIdAsync(carId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
    public class CreateDamageRequest
    {
        public int CarId { get; set; }
        public string Description { get; set; }
    }
}