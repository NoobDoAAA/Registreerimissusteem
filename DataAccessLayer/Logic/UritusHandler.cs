using DataAccessLayer.dto;
using DataAccessLayer.Interfaces;
using DataAccessLayer.ModelsDb;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccessLayer.Logic
{
    public class UritusHandler(MyDbContext context) : IDisposable
    {
        private readonly MyDbContext _dbContext = context;

        public void Dispose() => _dbContext.Dispose();


        private IQueryable<Uritus> GetAllUritused()
        {
            return context.Uritus.AsQueryable();
        }

        public async Task<IList<IUritus>> GetPlaneeritudUritused()
        {
            var dbUritused = await GetAllUritused().Where(u => !u.Kustutatud && u.Toimumisaeg > DateTime.Now).ToListAsync();

            var result = new List<IUritus>();

            foreach (var u in dbUritused)
            {
                result.Add(
                    item: new UritusDto
                    {
                        Id = u.Id,
                        Nimi = u.Nimi,
                        Toimumisaeg = u.Toimumisaeg,
                        ToimumiseKoht = u.ToimumiseKoht,
                        Lisainfo = u.Lisainfo
                    });
            }

            return result;
        }

        public async Task<IUritus?> GetUritusById(int Id)
        {
            var dbUritus = await _dbContext.Uritus.FindAsync(Id);

            return dbUritus == null
                ? null
                : new UritusDto
                {
                    Id = dbUritus.Id,
                    Nimi = dbUritus.Nimi,
                    Toimumisaeg = dbUritus.Toimumisaeg,
                    ToimumiseKoht = dbUritus.ToimumiseKoht,
                    Lisainfo = dbUritus.Lisainfo
                };
        }
    }
}
