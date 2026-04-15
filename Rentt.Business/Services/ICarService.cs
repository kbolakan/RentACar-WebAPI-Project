using Rentt.Core.DTOs;
using Rentt.Models;
using System.Threading.Tasks;

namespace Rentt.Business.Services
{
    public interface ICarService
    {
        Task<Car> AddCarAsync(AddCarDto carDto);
    }
}