﻿using DataAccessLayer.dto;
using DataAccessLayer.ModelsDb;
using DataAccessLayer.Interfaces;
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

        private IQueryable<Eraisik> GetAllEraisikud()
        {
            return _dbContext.Eraisik.AsQueryable();
        }

        private IQueryable<Ettevote> GetAllEttevotted()
        {
            return _dbContext.Ettevote.AsQueryable();
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

                var dbOsalejad = await GetAllOsalejad().Where(o => o.UritusId == Id && !o.Kustutatud).ToListAsync();

                foreach (var osaleja in dbOsalejad)
                {
                    osaleja.Kustutatud = true;
                }

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
        public async Task<IList<IEraisikOsaleja>> GetEraisikOsalejad(int UritusId)
        {
            var dbEraisikud = await GetAllEraisikOsalejad().Where(e => e.UritusId == UritusId).ToListAsync();

            var result = new List<IEraisikOsaleja>();

            foreach (var e in dbEraisikud)
            {
                result.Add(
                    item: new EraisikOsalejaDto
                    {
                        Id = e.Id,
                        EraisikId = e.EraisikId,
                        Eesnimi = e.Eesnimi,
                        Perekonnanimi = e.Perekonnanimi,
                        Isikukood = e.Isikukood
                    });
            }

            return result;
        }

        /// <summary>
        /// Ettevote osalejad
        /// </summary>
        public async Task<IList<IEttevoteOsaleja>> GetEttevoteOsalejad(int UritusId)
        {
            var dbEttevoted = await GetAllEttevoteOsalejad().Where(e => e.UritusId == UritusId).ToListAsync();

            var result = new List<IEttevoteOsaleja>();

            foreach (var e in dbEttevoted)
            {
                result.Add(
                    item: new EttevoteOsalejaDto
                    {
                        Id = e.Id,
                        EttevoteId = e.EttevoteId,
                        EttevoteNimi = e.EttevoteNimi,
                        Registrikood = e.Registrikood,
                        OsavotjateArv = e.OsavotjateArv
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
            await _dbContext.SaveChangesAsync();

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

        /// <summary>
        /// Lisa ettevote osaleja
        /// </summary>
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
            await _dbContext.SaveChangesAsync();

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

        /// <summary>
        /// Eemalda eraisik
        /// </summary>
        public async Task<bool> EemaldaEraisikOsaleja(int Id)
        {
            var dbOsaleja = await _dbContext.Osaleja.FindAsync(Id);

            if (dbOsaleja == null) return false;

            dbOsaleja.Kustutatud = true;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Eemalda ettevote
        /// </summary>
        public async Task<bool> EemaldaEttevoteOsaleja(int Id)
        {
            var dbOsaleja = await _dbContext.Osaleja.FindAsync(Id);

            if (dbOsaleja == null) return false;

            dbOsaleja.Kustutatud = true;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Eraisik osaleja
        /// </summary>
        public async Task<IEraisikOsaleja?> GetEraisikOsaleja(int Id)
        {
            var dbOsaleja = await _dbContext.Osaleja.FindAsync(Id);

            if (dbOsaleja == null) return null;

            var dbEraisik = await _dbContext.Eraisik.FindAsync(dbOsaleja.EraisikId);

            if (dbEraisik == null) return null;

            return new EraisikOsalejaDto
            {
                Id = dbOsaleja.Id,
                EraisikId = dbEraisik.Id,
                Eesnimi = dbEraisik.Eesnimi,
                Perekonnanimi = dbEraisik.Perekonnanimi,
                Isikukood = dbEraisik.Isikukood,
                MakseviisId = dbOsaleja.MakseviisId,
                Lisainfo = dbOsaleja.Lisainfo
            };
        }

        /// <summary>
        /// Muuda eraisik
        /// </summary>
        public async Task<bool> MuudaEraisikOsaleja(EraisikOsalejaDto? eraisik_osaleja)
        {
            if (eraisik_osaleja == null) return false;

            var dbOsaleja = await _dbContext.Osaleja.FindAsync(eraisik_osaleja.Id);

            if (dbOsaleja == null) return false;

            var dbEraisik = await _dbContext.Eraisik.FindAsync(eraisik_osaleja.EraisikId);

            if (dbEraisik == null) return false;

            dbEraisik.Eesnimi = eraisik_osaleja.Eesnimi;
            dbEraisik.Perekonnanimi = eraisik_osaleja.Perekonnanimi;
            dbEraisik.Isikukood = eraisik_osaleja.Isikukood;

            dbOsaleja.MakseviisId = eraisik_osaleja.MakseviisId;
            dbOsaleja.Lisainfo = eraisik_osaleja.Lisainfo;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Ettevote osaleja
        /// </summary>
        public async Task<IEttevoteOsaleja?> GetEttevoteOsaleja(int Id)
        {
            var dbOsaleja = await _dbContext.Osaleja.FindAsync(Id);

            if (dbOsaleja == null) return null;

            var dbEttevote = await _dbContext.Ettevote.FindAsync(dbOsaleja.EttevoteId);

            if (dbEttevote == null) return null;

            return new EttevoteOsalejaDto
            {
                Id = dbOsaleja.Id,
                EttevoteId = dbEttevote.Id,
                EttevoteNimi = dbEttevote.Nimi,
                Registrikood = dbEttevote.Registrikood,
                OsavotjateArv = dbOsaleja.OsavotjateArv,
                MakseviisId = dbOsaleja.MakseviisId,
                Lisainfo = dbOsaleja.Lisainfo
            };
        }

        /// <summary>
        /// Muuda ettevote
        /// </summary>
        public async Task<bool> MuudaEttevoteOsaleja(EttevoteOsalejaDto? ettevote_osaleja)
        {
            if (ettevote_osaleja == null) return false;

            var dbOsaleja = await _dbContext.Osaleja.FindAsync(ettevote_osaleja.Id);

            if (dbOsaleja == null) return false;

            var dbEttevote = await _dbContext.Ettevote.FindAsync(ettevote_osaleja.EttevoteId);

            if (dbEttevote == null) return false;

            dbEttevote.Nimi = ettevote_osaleja.EttevoteNimi;
            dbEttevote.Registrikood = ettevote_osaleja.Registrikood;

            dbOsaleja.OsavotjateArv = ettevote_osaleja.OsavotjateArv;
            dbOsaleja.MakseviisId = ettevote_osaleja.MakseviisId;
            dbOsaleja.Lisainfo = ettevote_osaleja.Lisainfo;

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
