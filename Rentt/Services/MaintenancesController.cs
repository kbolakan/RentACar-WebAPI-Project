using Microsoft.AspNetCore.Mvc;
using Rentt.Services;

namespace Rentt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenancesController : ControllerBase
    {
        private readonly IMaintenanceService _maintenanceService;

        public MaintenancesController(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendToMaintenance([FromBody] SendMaintenanceRequest request)
        {
            try
            {
                var result = await _maintenanceService.SendToMaintenanceAsync(request.CarId, request.Description);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("return/{maintenanceId}")] // Bu kez ID'yi URL'den alıyoruz, daha havalı!
        public async Task<IActionResult> ReturnFromMaintenance(int maintenanceId)
        {
            try
            {
                var result = await _maintenanceService.ReturnFromMaintenanceAsync(maintenanceId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class SendMaintenanceRequest
    {
        public int CarId { get; set; }
        public string Description { get; set; }
    }
}