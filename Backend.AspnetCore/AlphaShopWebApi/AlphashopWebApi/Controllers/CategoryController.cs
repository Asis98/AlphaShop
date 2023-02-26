using AlphashopWebApi.Dtos;
using AlphashopWebApi.Models;
using AlphashopWebApi.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AlphashopWebApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/category")]
    public class CategoryController: Controller
    {
        private readonly IArticoliRepository _articoliRepository;
        private readonly IMapper _mapper;

        public CategoryController(IArticoliRepository articoliRepository, IMapper mapper)
        {
            _articoliRepository = articoliRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        public async Task<ActionResult<CategoryDto>> GetCategory()
        {
            ICollection<FamilyAssort> category = await _articoliRepository.SelectCategory();

            return Ok(_mapper.Map<ICollection<CategoryDto>>(category));
        }
    }
}
