using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rentt.DataAccess;
using Rentt.Models;

namespace Rentt.Services
{
    public class DamageRecordService : IDamageRecordService
    {
        private readonly AppDbContext _context;

        public DamageRecordService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> CreateDamageRecordAsync(int carId, string description)
        {
            // KESİN ÇÖZÜM 1: _context.Cars aracı sınıfını KULLANMIYORUZ.
            // Doğrudan ana veritabanı motoruna (DbContext) emir veriyoruz.
            var car = await _context.FindAsync<Car>(carId);

            if (car == null)
            {
                throw new Exception("Sistemde böyle bir araç bulunamadı.");
            }

            var damageRecord = new DamageRecord
            {
                CarId = carId,
                Description = description
            };

            // KESİN ÇÖZÜM 2: _context.DamageRecords kullanmadan, doğrudan ana motora ekliyoruz.
            _context.Add(damageRecord);
            await _context.SaveChangesAsync();

            return new
            {
                Message = "Hasar kaydı başarıyla sisteme işlendi.",
                DamageId = damageRecord.Id
            };
        }

        public async Task<object> GetDamageRecordsByCarIdAsync(int carId)
        {
            // KESİN ÇÖZÜM 3: DbSet yerine doğrudan Set<T> metodu ile güvenli filtreleme yapıyoruz.
            var records = await _context.Set<DamageRecord>()
                .Where(d => d.CarId == carId)
                .ToListAsync();

            return records;
        }
    }
}