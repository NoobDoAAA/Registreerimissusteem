using DataAccessLayer.dto;
using DataAccessLayer.Interfaces;
using DataAccessLayer.ModelsDb;
using Microsoft.EntityFrameworkCore;

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

        private IQueryable<EraisikOsaleja> GetAllEraisikOsalejad()
        {
            return _dbContext.EraisikOsaleja.AsQueryable();
        }

        private IQueryable<EttevoteOsaleja> GetAllEttevoteOsalejad()
        {
            return _dbContext.EttevoteOsaleja.AsQueryable();
        }

        private IQueryable<Makseviis> GetAllMakseviisid()
        {
            return _dbContext.Makseviis.AsQueryable();
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
        public async Task<bool> LisaUritus(UritusDto? uritusDto)
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

            await _dbContext.Uritus.AddAsync(uritus);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> MuudaUritus(UritusDto? uritusDto)
        {
            if (uritusDto == null) return false;

            var dbUritus = await _dbContext.Uritus.FindAsync(uritusDto.Id);

            if (dbUritus == null) return false;

            dbUritus.Nimi = uritusDto.Nimi;
            dbUritus.Toimumisaeg = uritusDto.Toimumisaeg;
            dbUritus.ToimumiseKoht = uritusDto.ToimumiseKoht;
            dbUritus.Lisainfo = uritusDto.Lisainfo;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Urituse andmed
        /// </summary>
        public async Task<IUritus?> GetUritusById(int id)
        {
            var dbUritus = await _dbContext.Uritus.FindAsync(id);

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

        /// <summary>
        /// Eraisik osalejad
        /// </summary>
        public async Task<IList<IEraisikOsaleja>> GetEraisikOsalejad(int Id)
        {
            var dbEraisikud = await GetAllEraisikOsalejad().Where(e => e.Id == Id).ToListAsync();

            var result = new List<IEraisikOsaleja>();

            foreach (var e in dbEraisikud)
            {
                result.Add(
                    item: new EraisikOsalejaDto
                    {
                        Id = e.Id,
                        Eesnimi = e.Eesnimi,
                        Perekonnanimi = e.Perekonnanimi,
                        Isikukood = e.Isikukood,
                        Makseviis = e.Makseviis,
                        Lisainfo = e.Lisainfo
                    });
            }

            return result;
        }

        /// <summary>
        /// Ettevote osalejad
        /// </summary>
        public async Task<IList<IEttevoteOsaleja>> GetEttevoteOsalejad(int Id)
        {
            var dbEttevoted = await GetAllEttevoteOsalejad().Where(e => e.Id == Id).ToListAsync();

            var result = new List<IEttevoteOsaleja>();

            foreach (var e in dbEttevoted)
            {
                result.Add(
                    item: new EttevoteOsalejaDto
                    {
                        Id = e.Id,
                        EttevoteNimi = e.EttevoteNimi,
                        Registrikood = e.Registrikood,
                        Makseviis = e.Makseviis,
                        OsavotjateArv = e.OsavotjateArv,
                        Lisainfo = e.Lisainfo
                    });
            }

            return result;
        }

        /// <summary>
        /// Makseviisid
        /// </summary>
        public async Task<IList<IMakseviis>> GetMakseviisid()
        {
            var dbMakseviisid = await GetAllMakseviisid().Where(m => m.Aktiivne).ToListAsync();

            var result = new List<IMakseviis>();

            foreach (var m in dbMakseviisid)
            {
                result.Add(
                    item: new MakseviisDto
                    {
                        Id = m.Id,
                        Nimi = m.Nimi
                    });
            }

            return result;
        }

        /// <summary>
        /// Lisa eraisik osaleja
        /// </summary>
        public async Task<bool> LisaEraisikOsaleja(EraisikOsalejaDto? eraisik_osaleja)
        {
            if (eraisik_osaleja == null) return false;

            Eraisik eraisik = new()
            {
                Eesnimi = eraisik_osaleja.Eesnimi,
                Perekonnanimi = eraisik_osaleja.Perekonnanimi,
                Isikukood = eraisik_osaleja.Isikukood,
                Kustutatud = false
            };

            var dbEraisik = await _dbContext.Eraisik.AddAsync(eraisik);

            Osaleja osaleja = new()
            {
                UritusId = eraisik_osaleja.UritusId,
                EraisikId = dbEraisik.Entity.Id,
                EttevoteId = null,
                OsavotjateArv = 1,
                MakseviisId = eraisik_osaleja.MakseviisId,
                Lisainfo = eraisik_osaleja.Lisainfo,
                Kustutatud = false
            };

            await _dbContext.Osaleja.AddAsync(osaleja);
            await _dbContext.SaveChangesAsync();

            return true;
        }


        public async Task<bool> LisaEttevoteOsaleja(EttevoteOsalejaDto? ettevote_osaleja)
        {
            if (ettevote_osaleja == null) return false;

            Ettevote ettevote = new()
            {
                Nimi = ettevote_osaleja.EttevoteNimi,
                Registrikood = ettevote_osaleja.Registrikood,
                Kustutatud = false
            };

            var dbEttevote = await _dbContext.Ettevote.AddAsync(ettevote);

            Osaleja osaleja = new()
            {
                UritusId = ettevote_osaleja.UritusId,
                EraisikId = null,
                EttevoteId = dbEttevote.Entity.Id,
                OsavotjateArv = ettevote_osaleja.OsavotjateArv,
                MakseviisId = ettevote_osaleja.MakseviisId,
                Lisainfo = ettevote_osaleja.Lisainfo,
                Kustutatud = false
            };

            await _dbContext.Osaleja.AddAsync(osaleja);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
