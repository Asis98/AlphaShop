using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphashopWebApi.Dtos;
using AlphashopWebApi.Models;
using AlphashopWebApi.Services;
using ArticoliWebService.Dtos;
using ArticoliWebService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArticoliWebService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/articoli")]
    public class ArticoliController : Controller
    {
        private readonly IArticoliRepository _articolirepository;

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
            var articoli = await _articolirepository.SelArticoliByDescrizione(filter);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!articoli.Any())
            {
                return NotFound(
                    new ErrMsg(
                        string.Format(
                            "Non è stato trovato alcun articolo con il filtro '{0}'",
                            filter
                        ),
                        HttpContext.Response.StatusCode
                    )
                );
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
                return NotFound(
                    new ErrMsg(
                        string.Format("Non è stato trovato l'articolo con il codice '{0}'", CodArt),
                        HttpContext.Response.StatusCode
                    )
                );
            }

            var articolo = await _articolirepository.SelArticoloByCodice(CodArt);

            var barcodeDto = new List<BarcodeDto>();

            foreach (var ean in articolo.Barcode!)
            {
                barcodeDto.Add(new BarcodeDto { Barcode = ean.Barcode!, Tipo = ean.IdTipoArt! });
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
                return NotFound(
                    new ErrMsg(
                        string.Format("Non è stato trovato l'articolo con il barcode '{0}'", Ean),
                        HttpContext.Response.StatusCode
                    )
                );
            }

            return Ok(this.GetArticoliDto(articolo));
        }

        [HttpPost("inserisci")]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        [ProducesResponseType(201, Type = typeof(Articoli))]
        public async Task<IActionResult> SaveArticoli([FromBody] Articoli articolo)
        {
            if (articolo == null)
            {
                return BadRequest(
                    new ErrMsg("Dati nuovo articolo assenti", this.HttpContext.Response.StatusCode)
                );
            }

            // Controlliamo se l'articolo è presente
            var isPresent = await _articolirepository.ArticoloExists(articolo.CodArt);

            if (isPresent)
            {
                ModelState.AddModelError(
                    "",
                    $"Articolo {articolo.CodArt} presente in anagrafica! Impossibile inserirlo"
                );
                return StatusCode(422, ModelState);
            }
            // Verifichiamo che i dati siano corretti
            if (!ModelState.IsValid)
            {
                string ErrVal = "";
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        ErrVal += modelError.ErrorMessage + "|";
                    }
                }
                return BadRequest(ErrVal);
            }

            articolo.DataCreazione = DateTime.Today;

            var retVal = await _articolirepository.InsArticoli(articolo);
            if (!retVal)
            {
                ModelState.AddModelError(
                    "",
                    $"Ci sono stati problemi nell'inserimento dell'articolo {articolo.CodArt}"
                );
                return StatusCode(500, ModelState);
            }

            return Ok(
                this.GetArticoliDto(await _articolirepository.SelArticoloByCodice(articolo.CodArt))
            );
        }

        [HttpPut("modifica")]
        [ProducesResponseType(201, Type = typeof(Articoli))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateArticoli([FromBody] Articoli articolo)
        {
            if (articolo == null)
            {
                return BadRequest(
                    new ErrMsg("Dati Articolo Assenti", this.HttpContext.Response.StatusCode)
                );
            }

            string CodArt = (articolo.CodArt == null) ? "" : articolo.CodArt;

            //Contolliamo se l'articolo è presente
            var isPresent = await _articolirepository.ArticoloExists(CodArt);

            if (!isPresent)
            {
                ModelState.AddModelError(
                    "",
                    $"Articolo {CodArt} NON presente in anagrafica! Impossibile utilizzare il metodo PUT!"
                );
                return StatusCode(422, ModelState);
            }

            //Verifichiamo che i dati siano corretti
            if (!ModelState.IsValid)
            {
                string ErrVal = "";

                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        ErrVal += modelError.ErrorMessage + "|";
                    }
                }
                return BadRequest(ErrVal);
            }

            articolo.DataCreazione = DateTime.Today;

            var retVal = await _articolirepository.UpdArticoli(articolo);

            if (!retVal)
            {
                ModelState.AddModelError(
                    "",
                    $"Ci sono stati problemi nella modifica dell'Articolo {CodArt}.  "
                );
                return StatusCode(500, ModelState);
            }

            return Ok(this.GetArticoliDto(await _articolirepository.SelArticoloByCodice(CodArt)));
        }

        [HttpDelete("elimina/{codart}")]
        [ProducesResponseType(201, Type = typeof(InfoMsg))]
        [ProducesResponseType(400, Type = typeof(ErrMsg))]
        [ProducesResponseType(422, Type = typeof(ErrMsg))]
        [ProducesResponseType(500, Type = typeof(ErrMsg))]
        public async Task<IActionResult> DeleteArticoli(string codart)
        {
            if (codart == "")
            {
                return BadRequest(
                    new ErrMsg(
                        $"E' necessario inserire il codice dell'articolo da eliminare!",
                        this.HttpContext.Response.StatusCode
                    )
                );
            }

            Articoli articolo = await _articolirepository.DeleteArticoloByCodice(codart);

            if (articolo == null)
            {
                return StatusCode(
                    422,
                    new ErrMsg(
                        $"Articolo {codart} NON presente in anagrafica! Impossibile Eliminare!",
                        this.HttpContext.Response.StatusCode
                    )
                );
            }

            var retVal = await _articolirepository.DelArticoli(articolo);

            //verifichiamo che i dati siano stati regolarmente eliminati dal database
            if (!retVal)
            {
                return StatusCode(
                    500,
                    new ErrMsg(
                        $"Ci sono stati problemi nella eliminazione dell'Articolo {articolo.CodArt}.",
                        this.HttpContext.Response.StatusCode
                    )
                );
            }

            return Ok(
                new InfoMsg(
                    DateTime.Today,
                    $"Eliminazione articolo {codart} eseguita con successo!"
                )
            );
        }

        private ArticoliDto GetArticoliDto(Articoli articolo)
        {
            var barcodeDto = new List<BarcodeDto>();

            foreach (var ean in articolo.Barcode!)
            {
                barcodeDto.Add(new BarcodeDto { Barcode = ean.Barcode!, Tipo = ean.IdTipoArt! });
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
