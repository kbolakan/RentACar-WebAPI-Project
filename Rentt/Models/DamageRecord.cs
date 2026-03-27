using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rentt.Models
{
    public class DamageRecord
    {
        [Key]
        public int Id { get; set; }

        public int RentalId { get; set; }
        [ForeignKey("RentalId")]
        public Rental Rental { get; set; } = null!;

        [Required]
        public string Description { get; set; } = string.Empty;
        public decimal DamageCost { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime ReportDate { get; set; } = DateTime.UtcNow;
    }
}