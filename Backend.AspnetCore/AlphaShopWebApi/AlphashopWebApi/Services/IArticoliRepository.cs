using AlphashopWebApi.Models;

namespace AlphashopWebApi.Services
{
    public interface IArticoliRepository
    {
        public Task<IEnumerable<Articoli>> SelArticoliByDescrizione(string Descrizione);
        public Task<IEnumerable<Articoli>> SelArticoliByDescrizione(
            string Descrizione,
            string idCart
        );
        public Task<Articoli> SelArticoloByCodice(string Code);
        public Task<Articoli> DeleteArticoloByCodice(string Code);
        public Task<Articoli> SelArticoloByEan(string Ean);
        public Task<ICollection<Iva>> SelectIva();
        public Task<ICollection<FamilyAssort>> SelectCategory();
        public Task<bool> InsArticoli(Articoli articolo);
        public Task<bool> UpdArticoli(Articoli articolo);
        public Task<bool> DelArticoli(Articoli articolo);
        public Task<bool> ArticoloExists(string Code);
    }
}
