using System.ComponentModel.DataAnnotations;

namespace Rentt.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public byte[] PasswordHash { get; set; } // Şifrelenmiş veri

        [Required]
        public byte[] PasswordSalt { get; set; } // Şifreye eklenen eşsiz tuz (güvenlik artırıcı)

        public string? LicenseNumber { get; set; }
        public string? LicenseClass { get; set; }
        public DateTime? LicenseIssueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Role { get; set; } = "User"; // Varsayılan olarak herkes "User" olsun

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}