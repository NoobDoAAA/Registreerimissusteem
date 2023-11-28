namespace DataAccessLayer.Interfaces
{
    public interface IEttevoteOsaleja
    {
        public int Id { get; set; }
        public int UritusId { get; set; }
        public int EttevoteId { get; set; }
        public string EttevoteNimi { get; set; }
        public string Registrikood { get; set; }
        public int MakseviisId { get; set; }
        public string? Makseviis { get; set; }
        public int OsavotjateArv { get; set; }
        public string? Lisainfo { get; set; }
    }
}
