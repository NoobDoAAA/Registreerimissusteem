using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.ModelsDb
{
    public class EraisikOsaleja
    {
        [Key]
        public int Id { get; set; }
        public int UritusId { get; set; }
        public int EraisikId { get; set; }
        public required string Eesnimi { get; set; }
        public required string Perekonnanimi { get; set; }
        public required string Isikukood { get; set; }
        public int MakseviisId { get; set; }
        public required string Makseviis { get; set; }
        public string? Lisainfo { get; set; }
    }
}
