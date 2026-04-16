using Microsoft.AspNetCore.Mvc;
using Rentt.Business.Services;
using Rentt.Core.DTOs;
using System;
using System.Threading.Tasks;

namespace Rentt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

      
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

       
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
          
            if (await _authService.UserExists(userRegisterDto.Email))
            {
                return BadRequest("Bu e-posta adresi zaten sistemde kayıtlı!");
            }

       
            var createdUser = await _authService.Register(userRegisterDto);

          
            return Ok(new
            {
                Message = "Kullanıcı başarıyla şifrelendi ve kaydedildi!",
                Id = createdUser.Id, // <-- Son eklediğimiz 
                User = createdUser.FirstName + " " + createdUser.LastName
            });
        }

     
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
               
                var token = await _authService.Login(userLoginDto);

             
                return Ok(new
                {
                    Message = "Giriş başarılı! İşte dijital yaka kartınız:",
                    Token = token
                });
            }
            catch (Exception ex)
            {
              
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}