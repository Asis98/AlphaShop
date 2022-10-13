using Microsoft.AspNetCore.Mvc;

namespace AlphashopWebApi.Controllers
{
    [ApiController]
    [Route("api/saluti")]
    public class SalutiController
    {
        public string getSaluti()
        {
            return "Sono la tua prima web api creata in c#";
        }

    }
}
