namespace Rentt.Models
{
    public class DamageRecord
    {
        public int Id { get; set; }

        public int CarId { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Entity Framework İlişkisi
        public Car Car { get; set; }
    }
}