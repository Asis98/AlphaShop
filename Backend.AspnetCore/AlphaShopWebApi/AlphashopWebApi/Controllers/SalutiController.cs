using Microsoft.AspNetCore.Mvc;

namespace AlphashopWebApi.Controllers
{
    [ApiController]
    [Route("api/saluti")]
    public class SalutiController
    {
        public string getSaluti()
        {
            return "\"Sono la tua prima web api creata in c#\"";
        }

        [HttpGet("{Nome}")]
        public string getSaluti(string Nome) => string.Format("\"Saluti, {0} sono il webservice di aspentCore\"", Nome);

    }
}
