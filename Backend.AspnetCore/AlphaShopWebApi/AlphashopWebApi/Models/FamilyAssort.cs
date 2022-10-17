using System.ComponentModel.DataAnnotations;

namespace AlphashopWebApi.Models
{
    public class FamilyAssort
    {
        [Key]
        public int Id { get; set; }
        public string? Descrizione { get; set; }
        public virtual ICollection<Articoli>? Articoli { get; set; }
    }
}
