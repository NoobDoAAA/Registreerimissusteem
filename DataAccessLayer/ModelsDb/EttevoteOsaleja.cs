using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.ModelsDb
{
    public class EttevoteOsaleja
    {
        [Key]
        public int Id { get; set; }
        public required string EttevoteNimi { get; set; }
        public required string Registrikood { get; set; }
        public required string Makseviis { get; set; }
        public int OsavotjateArv { get; set; }
        public string? Lisainfo { get; set; }
    }
}
