using DataAccessLayer.dto;
using DataAccessLayer.Interfaces;
using DataAccessLayer.ModelsDb;
using Microsoft.EntityFrameworkCore;

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

        private IQueryable<Osaleja> GetAllOsalejad()
        {

            return context.Osaleja.AsQueryable();
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
