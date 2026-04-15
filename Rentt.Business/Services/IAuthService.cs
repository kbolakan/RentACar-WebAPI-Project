using Rentt.Core.DTOs;
using Rentt.Models;
using System.Threading.Tasks;

namespace Rentt.Business.Services // Senin klasör yapına uygun adres
{
    public interface IAuthService
    {
        Task<User> Register(UserRegisterDto userRegisterDto);
        Task<bool> UserExists(string email);
        Task<string> Login(UserLoginDto userLoginDto);
    }
}