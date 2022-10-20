using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphashopWebApi.Dtos;
using AlphashopWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArticoliWebService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/articoli")]
    public class ArticoliController : Controller
    {
        private IArticoliRepository articolirepository;

        public ArticoliController(IArticoliRepository articolirepository)
        {
            this.articolirepository = articolirepository;
        }

        [HttpGet("cerca/descrizione/{filter}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ArticoliDto>))]
        public async Task<IActionResult> GetArticoliByDesc(string filter)
        {
            var articoliDto = new List<ArticoliDto>();
            var articoli = await articolirepository.SelArticoliByDescrizione(filter) ;
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
                articoliDto.Add(new ArticoliDto 
                { 
                    CodArt = articolo.CodArt,
                    Descrizione = articolo.Descrizione,
                    Um = articolo.Um,
                    CodStat = articolo.CodStat,
                    PzCart = articolo.PzCart,
                    PesoNetto = articolo.PesoNetto,
                    DataCreazione = articolo.DataCreazione
                });
            }

            return Ok(articoliDto);
        }

    }
}