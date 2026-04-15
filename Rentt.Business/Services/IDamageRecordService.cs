namespace Rentt.Services
{
    public interface IDamageRecordService
    {
        // Hasar kaydı oluşturma komutu
        Task<object> CreateDamageRecordAsync(int carId, string description);

        // Bir arabanın tüm hasar geçmişini getirme komutu
        Task<object> GetDamageRecordsByCarIdAsync(int carId);
    }
}