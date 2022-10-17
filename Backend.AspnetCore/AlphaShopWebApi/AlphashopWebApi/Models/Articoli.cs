using System.ComponentModel.DataAnnotations;

namespace AlphashopWebApi.Models
{
    public class Articoli
    {
        //proprietà = nome colonna della tabella
        [Key]
        [MinLength(5, ErrorMessage ="Il numero minimi di caratteri è 5")]
        [MaxLength(30,  ErrorMessage ="Il numero massimo di caratteri è 30")]
        public string? CodArt { get; set; }
        [MinLength(5, ErrorMessage = "Il numero minimi di caratteri è 5")]
        [MaxLength(80, ErrorMessage = "Il numero massimo di caratteri è 80")] 
        public string? Descrizione { get; set; }
        public  string? Um { get; set; }
        public string? CodStat { get; set; }
        [Range(0,100, ErrorMessage ="I pezzi per  cartone devono essere compresi fra 0 e 100")]
        public int? PzCart { get; set; }
        [Range(0.01, 100, ErrorMessage = "I pezzi per  cartone devono essere compresi fra 0.1 e 100")]
        public double? PesoNetto { get; set; }
        public int? IdIva { get; set; }
        public int? IdFamAss { get; set; }
        public string? IdStatoArt { get; set; }
        public DateTime? DataCreazione { get; set; }

        //virtual carica la tabella collegata solo quando serve
        //ad un articolo corrisponde molti barcode
        public virtual ICollection<Ean>? Barcode { get; set; }

        //relazione 1 a 1
        public virtual Ingredienti? ingredienti { get; set; }

        public virtual Iva? iva { get; set; }

        public  virtual FamilyAssort? familyAssort { get; set; }
    }
}
