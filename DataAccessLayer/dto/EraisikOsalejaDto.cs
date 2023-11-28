using DataAccessLayer.Interfaces;

namespace DataAccessLayer.dto
{
    public class EraisikOsalejaDto : IEraisikOsaleja
    {
        public int Id { get; set; }
        public int UritusId { get; set; }
        public int EraisikId { get; set; }
        public required string Eesnimi { get; set; }
        public required string Perekonnanimi { get; set; }
        public required string Isikukood { get; set; }
        public int MakseviisId { get; set; }
        public string? Makseviis { get; set; }
        public string? Lisainfo { get; set; }
    }
}
