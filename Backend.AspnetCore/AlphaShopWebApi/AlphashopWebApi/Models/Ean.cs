using System.ComponentModel.DataAnnotations;

namespace AlphashopWebApi.Models
{
    public class Ean
    {
        public string? CodArt { get; set; }
        [Key]
        [StringLength(13, MinimumLength =8, ErrorMessage ="Il Barcode deve avere da 8 a 13 cifre")]
        public string? Barcode { get; set; }
        [Required]
        public string? IdTipoArt { get; set; }

        //relazione molti ad uno, ad un barcode corrisponde un solo articolo
        public virtual Articoli? Articolo { get; set; }
    }
}
