using DataAccessLayer.dto;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Logic
{
    public class UritusHandler(MyDbContext context) : IDisposable
    {
        private readonly MyDbContext _dbContext = context;

        public void Dispose() => _dbContext.Dispose();

        public IUritus? GetUritusById(int Id)
        {
            var dbUritus = _dbContext.Uritus.Find(Id);

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
