namespace Rentt.Core.DTOs
{
    public class AddCarDto
    {
        public string Brand { get; set; } // Marka (Örn: BMW)
        public string Model { get; set; } // Model (Örn: 320i)
        public int Year { get; set; } // Yıl
        public decimal DailyPrice { get; set; } // Günlük Kiralama Ücreti
        public decimal ExtraKmFee { get; set; } // Kilometre Aşım Ücreti
        public int CurrentMileage { get; set; } // Şu anki Kilometresi
        public int MaintenanceMileageThreshold { get; set; } // Kaç km'de bakıma gireceği
    }
}