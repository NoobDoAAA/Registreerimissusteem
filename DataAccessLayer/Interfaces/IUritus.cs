namespace DataAccessLayer.Interfaces
{
    public interface IUritus
    {
        public int Id { get; set; }
        public string Nimi { get; set; }
        public DateTime Toimumisaeg { get; set; }
        public string ToimumiseKoht { get; set; }
        public string? Lisainfo { get; set; }
    }
}
