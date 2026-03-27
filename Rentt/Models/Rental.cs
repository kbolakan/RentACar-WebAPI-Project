using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rentt.Models
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public int CarId { get; set; }
        [ForeignKey("CarId")]
        public Car Car { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public int StartMileage { get; set; }
        public int? ReturnMileage { get; set; }

        public decimal BasePrice { get; set; }
        public decimal ExtraKmPrice { get; set; }
        public decimal LateFee { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Active";

        public ICollection<DamageRecord> DamageRecords { get; set; } = new List<DamageRecord>();
    }
}