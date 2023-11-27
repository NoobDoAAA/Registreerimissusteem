using DataAccessLayer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class EraisikOsalejaViewModel : IEraisikOsaleja
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public int UritusId { get; set; }

        [Required]
        [StringLength(50)]
        public required string Eesnimi { get; set; }

        [Required]
        [StringLength(50)]
        public required string Perekonnanimi { get; set; }

        [Required]
        [StringLength(11)]
        public required string Isikukood { get; set; }

        [ScaffoldColumn(false)]
        public int MakseviisId { get; set; }

        [StringLength(50)]
        public string? Makseviis { get; set; }

        [StringLength(1500)]
        public string? Lisainfo { get; set; }
    }
}
