using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.ModelsDb
{
    public class EraisikOsaleja
    {
        [Key]
        public int Id { get; set; }
        public required string Eesnimi { get; set; }
        public required string Perekonnanimi { get; set; }
        public required string Isikukood { get; set; }
        public required string Makseviis { get; set; }
        public string? Lisainfo { get; set; }
    }
}
