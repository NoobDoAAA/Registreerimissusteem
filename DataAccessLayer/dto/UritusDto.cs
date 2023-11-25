using DataAccessLayer.Interfaces;

namespace DataAccessLayer.dto
{
    public class UritusDto : IUritus
    {
        public int Id { get; set; }
        public required string Nimi { get; set; }
        public DateTime Toimumisaeg { get; set; }
        public required string ToimumiseKoht { get; set; }
        public int OsavotjateArv { get; set; }
        public string? Lisainfo { get; set; }
        public bool Kustutatud { get; set; }
    }
}
