using DataAccessLayer.dto;
using DataAccessLayer.Interfaces;
using DataAccessLayer.ModelsDb;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Logic
{
    public class UritusHandler : IDisposable
    {
        private readonly MyDbContext _dbContext;

        public UritusHandler(MyDbContext context)
        {
            _dbContext = context;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        private IQueryable<Uritus> GetAllUritused()
        {
            return _dbContext.Uritus.AsQueryable();
        }

        private IQueryable<Osaleja> GetAllOsalejad()
        {
            return _dbContext.Osaleja.AsQueryable();
        }

        private async Task<int> ArvutaOsavotjateArv(int UritusId)
        {
            return await GetAllOsalejad().Where(o => o.UritusId == UritusId).SumAsync(o => o.OsavotjateArv);
        }

        /// <summary>
        /// Planeeritud uritused nimekiri
        /// </summary>
        public async Task<IList<IUritus>> GetPlaneeritudUritused()
        {
            var dbUritused = await GetAllUritused().Where(u => !u.Kustutatud && u.Toimumisaeg > DateTime.Now).ToListAsync();

            var result = new List<IUritus>();

            foreach (var u in dbUritused)
            {
                var arv = ArvutaOsavotjateArv(u.Id);

                arv.Wait();

                result.Add(
                    item: new UritusDto
                    {
                        Id = u.Id,
                        Nimi = u.Nimi,
                        Toimumisaeg = u.Toimumisaeg,
                        ToimumiseKoht = u.ToimumiseKoht,
                        Lisainfo = u.Lisainfo,
                        OsavotjateArv = arv.Result
                    });
            }

            return result;
        }

        /// <summary>
        /// Moodunud uritused nimekiri
        /// </summary>
        public async Task<IList<IUritus>> GetMoodunudUritused()
        {
            var dbUritused = await GetAllUritused().Where(u => !u.Kustutatud && u.Toimumisaeg < DateTime.Now).ToListAsync();

            var result = new List<IUritus>();

            foreach (var u in dbUritused)
            {
                var arv = ArvutaOsavotjateArv(u.Id);

                arv.Wait();

                result.Add(
                    item: new UritusDto
                    {
                        Id = u.Id,
                        Nimi = u.Nimi,
                        Toimumisaeg = u.Toimumisaeg,
                        ToimumiseKoht = u.ToimumiseKoht,
                        Lisainfo = u.Lisainfo,
                        OsavotjateArv = arv.Result
                    });
            }

            return result;
        }

        /// <summary>
        /// Urituse tuhistamine
        /// </summary>
        public async Task<bool> EemaldaUritus(int Id)
        {
            var dbUritus = await _dbContext.Uritus.FindAsync(Id);

            if (dbUritus != null)
            {
                dbUritus.Kustutatud = true;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Urituse lisamine
        /// </summary>
        public bool LisaUritus(UritusDto? uritusDto)
        {
            if (uritusDto == null) return false;

            Uritus uritus = new()
            {
                Nimi = uritusDto.Nimi,
                Toimumisaeg = uritusDto.Toimumisaeg,
                ToimumiseKoht = uritusDto.ToimumiseKoht,
                Lisainfo = uritusDto.Lisainfo,
                Kustutatud = false
            };

            _dbContext.Uritus.Add(uritus);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
