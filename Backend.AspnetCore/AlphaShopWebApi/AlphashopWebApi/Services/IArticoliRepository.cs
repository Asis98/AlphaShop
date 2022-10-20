using AlphashopWebApi.Models;

namespace AlphashopWebApi.Services
{
    public interface IArticoliRepository
    {
        public Task<IEnumerable<Articoli>> SelArticoliByDescrizione(string Descrizione);
        public Articoli SelArticoloByCodice(string Code);
        public Articoli SelArticoloByEan(string Ean);
        public bool InsArticoli(Articoli articolo);
        public bool UpdArticoli(Articoli articolo);
        public bool DelArticoli(Articoli articolo);
        public bool Salva();
        public Task<bool> ArticoloExists(string Code);
    }
}
