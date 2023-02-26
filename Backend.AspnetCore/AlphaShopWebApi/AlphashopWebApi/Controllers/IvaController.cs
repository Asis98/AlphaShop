using AlphashopWebApi.Dtos;
using AlphashopWebApi.Models;
using AlphashopWebApi.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AlphashopWebApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/iva")]
    public class IvaController : Controller
    {
        private readonly IArticoliRepository _articoliRepository;
        private readonly IMapper _mapper;

        public IvaController(IArticoliRepository articoliRepository, IMapper mapper)
        {
            _articoliRepository = articoliRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<IvaDto>))]
        public async Task<ActionResult<IvaDto>> GetIva()
        {
            ICollection<Iva> iva = await _articoliRepository.SelectIva();

            return Ok(_mapper.Map<ICollection<IvaDto>>(iva));
        }

    }
}
