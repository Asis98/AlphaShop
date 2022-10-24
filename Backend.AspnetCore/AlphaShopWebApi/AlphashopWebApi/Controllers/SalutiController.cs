using AlphashopWebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AlphashopWebApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/saluti")]
    public class SalutiController : Controller
    {

        [HttpGet("{Nome}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(string))]
        public string getSaluti(string Nome) => string.Format("\"Saluti, {0} sono il webservice di aspentCore\"", Nome);

    }
}
