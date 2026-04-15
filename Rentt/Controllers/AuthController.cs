using Microsoft.AspNetCore.Mvc;
using Rentt.Business.Services;
using Rentt.Core.DTOs;
using System; // Try-Catch içindeki Exception için eklendi
using System.Threading.Tasks;

namespace Rentt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        // Dependency Injection ile yazdığımız güvenlik servisini içeri alıyoruz
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // --- 1. KAYIT OLMA (REGISTER) UÇ NOKTASI ---
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            // Bu email ile daha önce kayıt olunmuş mu?
            if (await _authService.UserExists(userRegisterDto.Email))
            {
                return BadRequest("Bu e-posta adresi zaten sistemde kayıtlı!");
            }

            // Yeni kullanıcıyı şifreleyerek sisteme kaydet
            var createdUser = await _authService.Register(userRegisterDto);

            return Ok(new
            {
                Message = "Kullanıcı başarıyla şifrelendi ve kaydedildi!",
                User = createdUser.FirstName + " " + createdUser.LastName
            });
        }

        // --- 2. GİRİŞ YAPMA (LOGIN) UÇ NOKTASI (YENİ EKLENDİ) ---
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                // Güvenlik servisimiz şifreyi çözecek ve her şey doğruysa Token üretecek
                var token = await _authService.Login(userLoginDto);

                // Başarılı olursa o upuzun dijital yaka kartını (JWT) müşteriye veriyoruz
                return Ok(new
                {
                    Message = "Giriş başarılı! İşte dijital yaka kartınız:",
                    Token = token
                });
            }
            catch (Exception ex)
            {
                // Eğer şifre yanlışsa veya kullanıcı yoksa, yazdığımız o net hata mesajını göster
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}