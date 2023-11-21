namespace DataAccessLayer.ModelsDb
{
    public class Osaleja
    {
        public int Id { get; set; }
        public int UritusId { get; set; }
        public int? EraisikId { get; set; }
        public int? EttevoteId { get; set; }
        public int OsavotjateArv { get; set; }
        public int MakseviisId { get; set; }
        public string? Lisainfo { get; set; }
        public bool Kustutatud { get; set; }
    }
}
