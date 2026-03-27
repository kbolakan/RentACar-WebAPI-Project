namespace Rentt.Services
{
    public interface IMaintenanceService
    {
        // Aracı bakıma gönderen komut
        Task<object> SendToMaintenanceAsync(int carId, string description);

        // Aracı bakımdan çıkaran komut
        Task<object> ReturnFromMaintenanceAsync(int maintenanceId);
    }
}