using System.Threading.Tasks;

namespace Rentt.Business.Abstract
{
    public interface IMaintenanceService
    {
        // SADECE BU İKİSİ OLMALI:
        Task<object> SendToMaintenanceAsync(int carId, string description);
        Task<object> ReturnFromMaintenanceAsync(int maintenanceId);
    }
}