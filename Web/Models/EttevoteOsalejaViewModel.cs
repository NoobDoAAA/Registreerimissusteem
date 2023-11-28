using DataAccessLayer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class EttevoteOsalejaViewModel : IEttevoteOsaleja
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [ScaffoldColumn(false)]
        public int UritusId { get; set; }

        [ScaffoldColumn(false)]
        public int EttevoteId { get; set; }

        [Required]
        [StringLength(50)]
        public required string EttevoteNimi { get; set; }

        [Required]
        [StringLength(50)]
        public required string Registrikood { get; set; }

        [ScaffoldColumn(false)]
        public int MakseviisId { get; set; }

        [StringLength(50)]
        public string? Makseviis { get; set; }

        [Required]
        public int OsavotjateArv { get; set; }

        [StringLength(5000)]
        public string? Lisainfo { get; set; }
    }
}
