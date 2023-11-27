using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.ModelsDb
{
    public class Ettevote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Nimi { get; set; }
        public required string Registrikood { get; set; }
        public bool Kustutatud { get; set; }
    }
}
