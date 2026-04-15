using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // AppSettings'i okumak için
using Microsoft.IdentityModel.Tokens; // Token oluşturmak için
using Rentt.Core.DTOs;
using Rentt.DataAccess;
using Rentt.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rentt.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration; // Gizli anahtarı okuyan alet

        // Constructor'a IConfiguration eklendi
        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> Login(UserLoginDto userLoginDto)
        {
            // 1. Kullanıcıyı Email'den bul
            var user = await _context.Set<User>().FirstOrDefaultAsync(x => x.Email == userLoginDto.Email);
            if (user == null) throw new Exception("Kullanıcı bulunamadı.");

            // 2. Şifresi doğru mu kontrol et
            if (!VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Şifre hatalı.");

            // 3. Her şey doğruysa Token (Yaka Kartı) oluştur ve yolla
            return CreateToken(user);
        }

        // --- DİĞER METOTLAR BURADA DURUYOR ---
        public async Task<User> Register(UserRegisterDto userRegisterDto)
        {
            CreatePasswordHash(userRegisterDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User
            {
                FirstName = userRegisterDto.FirstName,
                LastName = userRegisterDto.LastName,
                Email = userRegisterDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "User"
            };
            _context.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Set<User>().AnyAsync(x => x.Email == email);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        // --- ŞİFREYİ ÇÖZEN VE DOĞRULAYAN YENİ METOT ---
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        // --- YAKA KARTINI (JWT TOKEN) BASAN YENİ METOT ---
        private string CreateToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1), // Kart 1 gün geçerli
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}