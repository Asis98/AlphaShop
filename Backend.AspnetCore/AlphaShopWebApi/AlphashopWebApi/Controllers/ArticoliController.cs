using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphashopWebApi.Dtos;
using AlphashopWebApi.Models;
using AlphashopWebApi.Services;
using ArticoliWebService.Dtos;
using ArticoliWebService.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ArticoliWebService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/articoli")]
    public class ArticoliController : Controller
    {
        private readonly IArticoliRepository _articolirepository;
        private readonly IMapper _mapper;

        public ArticoliController(IArticoliRepository articolirepository, IMapper mapper)
        {
            _articolirepository = articolirepository;
            _mapper = mapper;
        }

        [HttpGet("cerca/descrizione/{filter}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ArticoliDto>))]
        public async Task<ActionResult<IEnumerable<ArticoliDto>>> GetArticoliByDesc(
            string filter,
            [FromQuery(Name = "cat")] string? idCat,
            [FromQuery(Name = "prz")] double? prezzo
        )
        {
            var articoliDto = new List<ArticoliDto>();
            var articoli = await _articolirepository.SelArticoliByDescrizione(filter, idCat);
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticoliDto))]
        public async Task<ActionResult<ArticoliDto>> GetArticoloByCode(string CodArt)
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticoliDto))]
        public async Task<ActionResult<ArticoliDto>> GetArticoloByEan(string Ean)
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Articoli))]
        public async Task<ActionResult<Articoli>> SaveArticoli([FromBody] Articoli articolo)
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Articoli))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Articoli>> UpdateArticoli([FromBody] Articoli articolo)
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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ErrMsg))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrMsg))]
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

            foreach (var ean in articolo.Barcode)
            {
                barcodeDto.Add(new BarcodeDto { Barcode = ean.Barcode, Tipo = ean.IdTipoArt });
            }

            ArticoliDto articoliDto = _mapper.Map<ArticoliDto>(articolo);

            articoliDto.Ean = barcodeDto;

            return articoliDto;
        }
    }
}
