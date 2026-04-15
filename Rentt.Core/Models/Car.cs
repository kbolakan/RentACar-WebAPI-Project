using System.ComponentModel.DataAnnotations;

namespace Rentt.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public string Model { get; set; } = string.Empty;

        public int Year { get; set; }
        public int CurrentMileage { get; set; }
        public int MaintenanceMileageThreshold { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal ExtraKmFee { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
        public ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();
    }
}