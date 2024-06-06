using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    // api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository) {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        //CREATE WALK
        //POST: /api/walks
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDTO addWalkRequestDTO) 
        {
            //Map DTO to Domain Model
            var walkDomainModel =  mapper.Map<Walk>(addWalkRequestDTO);
            await walkRepository.CreateAsync(walkDomainModel);
            //Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }
    }
}
