namespace DataAccessLayer.Interfaces
{
    public interface IEraisikOsaleja
    {
        public int Id { get; set; }
        public int UritusId { get; set; }
        public string Eesnimi { get; set; }
        public string Perekonnanimi { get; set; }
        public string Isikukood { get; set; }
        public int MakseviisId { get; set; }
        public string Makseviis { get; set; }
        public string? Lisainfo { get; set; }
    }
}
