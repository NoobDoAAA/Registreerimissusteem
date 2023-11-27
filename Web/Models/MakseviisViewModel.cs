using DataAccessLayer.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class MakseviisViewModel : IMakseviis
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Nimi { get; set; }
    }
}
