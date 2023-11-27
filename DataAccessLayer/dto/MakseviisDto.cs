using DataAccessLayer.Interfaces;

namespace DataAccessLayer.dto
{
    public class MakseviisDto : IMakseviis
    {
        public int Id { get; set; }
        public required string Nimi { get; set; }
    }
}
