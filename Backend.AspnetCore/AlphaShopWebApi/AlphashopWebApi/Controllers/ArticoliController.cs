using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphashopWebApi.Dtos;
using AlphashopWebApi.Models;
using AlphashopWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArticoliWebService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/articoli")]
    public class ArticoliController : Controller
    {
        private IArticoliRepository _articolirepository;

        public ArticoliController(IArticoliRepository articolirepository)
        {
            this._articolirepository = articolirepository;
        }

        [HttpGet("cerca/descrizione/{filter}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ArticoliDto>))]
        public async Task<IActionResult> GetArticoliByDesc(string filter)
        {
            var articoliDto = new List<ArticoliDto>();
            var articoli = await _articolirepository.SelArticoliByDescrizione(filter) ;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!articoli.Any())
            {
                return NotFound(string.Format("Non è stato trovato alcun articolo con il filtro '{0}'", filter));
            }

            foreach (var articolo in articoli)
            {
                articoliDto.Add(GetArticoliDto(articolo));
            }

            return Ok(articoliDto);
        }
        [HttpGet("cerca/codice/{CodArt}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ArticoliDto))]
        public async Task<IActionResult> GetArticoloByCode(string CodArt)
        {
            bool retVal = await _articolirepository.ArticoloExists(CodArt);

            if (!retVal)
            {
                return NotFound(string.Format("Non è stato trovato l'articolo con il codice '{0}'", CodArt));
            }

            var articolo = await _articolirepository.SelArticoloByCodice(CodArt);

            var barcodeDto = new List<BarcodeDto>();

            foreach (var ean in articolo.Barcode!)
            {
                barcodeDto.Add(new BarcodeDto { 
                    Barcode = ean.Barcode!,
                    Tipo = ean.IdTipoArt!

                });
            }

            return Ok(this.GetArticoliDto(articolo));
        }
        [HttpGet("cerca/barcode/{Ean}")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ArticoliDto))]
        public async Task<IActionResult> GetArticoloByEan(string Ean)
        {
            var articolo = await _articolirepository.SelArticoloByEan(Ean);

            if (articolo == null)
            {
                return NotFound(string.Format("Non è stato trovato l'articolo con il barcode '{0}'", Ean));
            }

            return Ok(this.GetArticoliDto(articolo));
        }

        private ArticoliDto GetArticoliDto(Articoli articolo)
        {
            var barcodeDto = new List<BarcodeDto>();

            foreach (var ean in articolo.Barcode!)
            {
                barcodeDto.Add(new BarcodeDto
                {
                    Barcode = ean.Barcode!,
                    Tipo = ean.IdTipoArt!
                });
            }

            var articoliDto = new ArticoliDto
            {
                CodArt = articolo.CodArt,
                Descrizione = articolo.Descrizione,
                Um = articolo.Um?.Trim(),
                CodStat = articolo.CodStat?.Trim(),
                PzCart = articolo.PzCart,
                PesoNetto = articolo.PesoNetto,
                DataCreazione = articolo.DataCreazione,
                IdStatoArticolo = articolo.IdStatoArt!,
                Ean = barcodeDto,
                Iva = new IvaDto(articolo.iva!.Descrizione!, articolo.iva.Aliquota),
                Categoria = articolo.familyAssort!.Descrizione!,
            };

            return articoliDto;
        }
    }
}