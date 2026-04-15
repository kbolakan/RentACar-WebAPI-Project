using Microsoft.AspNetCore.Mvc;
using Rentt.Business.Abstract; // <-- İŞTE O HAYAT KURTARAN DOĞRU ADRES
using System;
using System.Threading.Tasks;

namespace Rentt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenancesController : ControllerBase
    {
        private readonly IMaintenanceService _maintenanceService;

        // Dependency Injection ile bakım sözleşmemizi (interface) içeri alıyoruz
        public MaintenancesController(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
        }

        // Aracı bakıma gönderen uç nokta (Endpoint)
        [HttpPost("send")]
        public async Task<IActionResult> SendToMaintenance([FromBody] SendMaintenanceRequest request)
        {
            try
            {
                var result = await _maintenanceService.SendToMaintenanceAsync(request.CarId, request.Description);
                return Ok(result); // 200 Başarılı
            }
            catch (Exception ex)
            {
                // Eğer araç bulunamazsa veya zaten kiradaysa yazdığımız hata mesajını döndür
                return BadRequest(new { Error = ex.Message });
            }
        }

        // Aracı bakımdan çıkaran uç nokta (Endpoint)
        [HttpPost("return/{maintenanceId}")]
        public async Task<IActionResult> ReturnFromMaintenance(int maintenanceId)
        {
            try
            {
                var result = await _maintenanceService.ReturnFromMaintenanceAsync(maintenanceId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }

    // Eğer projende SendMaintenanceRequest diye bir DTO yoksa hata vermesin diye:
    public class SendMaintenanceRequest
    {
        public int CarId { get; set; }
        public string Description { get; set; }
    }
}