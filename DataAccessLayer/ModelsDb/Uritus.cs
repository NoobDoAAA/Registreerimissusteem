using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.ModelsDb
{
    public class Uritus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Nimi { get; set; }
        public DateTime Toimumisaeg { get; set; }
        public required string ToimumiseKoht { get; set; }
        public string? Lisainfo { get; set; }
        public bool Kustutatud { get; set; }
    }
}
