using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.ModelsDb
{
    public class Eraisik
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Eesnimi { get; set; }
        public required string Perekonnanimi { get; set; }
        public required string Isikukood { get; set; }
        public bool Kustutatud { get; set; }
    }
}
