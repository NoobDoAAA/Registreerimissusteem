using DataAccessLayer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class UritusViewModel : IUritus
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Nimi { get; set; }

        [Required]
        public DateTime Toimumisaeg { get; set; }

        [Required]
        [StringLength(100)]
        public required string ToimumiseKoht { get; set; }

        [ScaffoldColumn(false)]
        public int OsavotjateArv { get; set; }

        [StringLength(1000)]
        public string? Lisainfo { get; set; }

        [ScaffoldColumn(false)]
        public bool Kustutatud { get; set; }
    }
}
