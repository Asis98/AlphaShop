using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphashopWebApi.Models;
using AlphashopWebApi.Services;
using Microsoft.EntityFrameworkCore;

namespace ArticoliWebService.Services
{
    public class ArticoliRepository : IArticoliRepository
    {
        AlphaShopDbContext _alphaShopDbContext;

        public ArticoliRepository(AlphaShopDbContext alphaShopDbContext)
        {
            _alphaShopDbContext =  alphaShopDbContext;
        }

        public async Task<IEnumerable<Articoli>> SelArticoliByDescrizione(string Descrizione) =>
                await _alphaShopDbContext.Articoli
                .Where(a => a.Descrizione!.Contains(Descrizione))
                .Include(a => a.Barcode)
                .Include(a => a.iva)
                .Include(a => a.familyAssort)
                .OrderBy(a => a.Descrizione)
                .ToListAsync();

        public async Task<Articoli> SelArticoloByCodice(string Code) =>
                await _alphaShopDbContext.Articoli
                    .Where(a => a.CodArt!.Equals(Code))
                    .Include(a => a.Barcode)
                    .Include(a => a.iva)
                    .Include(a => a.familyAssort)
                    .FirstOrDefaultAsync()!;

        public async Task<Articoli> SelArticoloByEan(string Ean) =>
                await _alphaShopDbContext.Barcode
                    .Where(b => b.Barcode!.Equals(Ean))
                        .Include(a => a.Articolo!.Barcode!)
                        .Include(a => a.Articolo!.familyAssort!)
                        .Include(a => a.Articolo!.iva!)
                    .Select(a => a.Articolo)
                    .FirstOrDefaultAsync()!;

        public async Task<bool> InsArticoli(Articoli articolo)
        {
           await _alphaShopDbContext.AddAsync(articolo);
           return await Salva();
        }
        public async Task<bool> UpdArticoli(Articoli articolo)
        {
            _alphaShopDbContext.Update(articolo);
            return await Salva();
        }

        public async Task<bool> DelArticoli(Articoli articolo)
        {
            _alphaShopDbContext.Remove(articolo);
            return await Salva();
        }
        private async Task<bool> Salva()
        {
            var saved = await _alphaShopDbContext.SaveChangesAsync();
            return saved >=0 ? true : false;
        }
        public async Task<bool> ArticoloExists(string Code) =>
            await _alphaShopDbContext.Articoli
                .AnyAsync(c => c.CodArt == Code);

    }
}