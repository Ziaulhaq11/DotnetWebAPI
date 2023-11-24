using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    //https://localhost:portNumber/api/Regions
    [Route("api/[controller]")]
    [ApiController] //will automatically tell that its for API use. And it valdiates modal state and gives 400 response to the caller
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext DbContext;
        private readonly IRegionRepository regionRepository;

        //Now we're using RegionRepository, so no need of DbContext;
        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
        {
            this.DbContext = dbContext;
            this.regionRepository = regionRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            //Now In Regions when we hover we can see "Domain Model Region". So we're sending Domain Modle to client which is not a good practice;
            //Get Data from Database -- Domain Models here
            //var regions = DbContext.Regions.ToList();
            //Making this method async
            //Entity Framework provides Async functionality to all the methods
            var regions = await regionRepository.GetAllAsync();
            //Map Domain Models to DTO's
            var regionsDto = new List<RegionDto>();
            foreach (var region in regions)
            {
                regionsDto.Add(new RegionDto
                {
                    Id = region.Id,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl,
                    Name = region.Name
                }); ;

            }

            //Return DTOs
            return Ok(regionsDto);
        }

        //Get REgion by ID
        [HttpGet]
        [Route("{id:Guid}")] //Type not required to pass
        public async Task<IActionResult> GetById(Guid id) //([FromRoute] Guid id) Its working without FromRoute as well.not int--got error bc of this
        {
            //var region = DbContext.Regions.Find(id);
            var region = await regionRepository.GetByIdAsync(id); //It will work if we're searching with Name or something else.
            if (region == null)
            {
                return NotFound();
            }

            return Ok(region);
        }

        //POST : same route as above, but when user selects POST, it will come here
        //In Post method we have FromBody and giving type what is coming.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionDto addRegionRequestDTO)
        {
            //Map or Convert DTO to Domain Model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl
            };

            //Use Domain Model to create Region

            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //Map Domain Model back to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            //For post 201 code should send. And we are sending the created Database data
            //here we're indirectly calling GetByID Method with the ID
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        //PUT
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateRegionDto regionData)
        {
            //Map DTO To Domain Model
            var regionDomainModel = new Region
            {
                Code = regionData.Code,
                RegionImageUrl = regionData.RegionImageUrl,
                Name = regionData.Name,
            };
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //Convert Domain Model to DTO
            var regionDTO = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDTO);
        }

        //DELETE
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}

//BEFORE IMPLEMENTING REPOSITORY PATTERNS
/*
 * namespace NZWalksAPI.Controllers
    {
    //https://localhost:portNumber/api/Regions
    [Route("api/[controller]")]
    [ApiController] //will automatically tell that its for API use. And it valdiates modal state and gives 400 response to the caller
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext DbContext;

        //public RegionsController(NZWalksDbContext dbContext)
        
        public RegionsController(NZWalksDbContext dbContext)
        {
            this.DbContext = dbContext;
        }
        [HttpGet]
        //public ActionResult GetAll() -- Without Async
        public async Task<IActionResult> GetAll()
        {
            //var regions = new List<Region>
            //{
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Auckland Region",
            //        Code = "AKL",
            //        RegionImageUrl = "https://images.pexels.com/photos/17963791/pexels-photo-17963791.jpeg?cs=srgb&dl=pexels-wladislawa-schr%C3%B6der-17963791.jpg&fm=jpg&w=4000&h=6000&_gl=1*1qhxymr*_ga*MTQ3OTQ3MDE4OC4xNjk5MzY4MjQx*_ga_8JE65Q40S6*MTY5OTM2ODI0My4xLjAuMTY5OTM2ODI0My4wLjAuMA"
            //    },
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Wellington Region",
            //        Code = "WLG",
            //        RegionImageUrl = "https://images.pexels.com/photos/17963791/pexels-photo-17963791.jpeg?cs=srgb&dl=pexels-wladislawa-schr%C3%B6der-17963791.jpg&fm=jpg&w=4000&h=6000&_gl=1*1qhxymr*_ga*MTQ3OTQ3MDE4OC4xNjk5MzY4MjQx*_ga_8JE65Q40S6*MTY5OTM2ODI0My4xLjAuMTY5OTM2ODI0My4wLjAuMA"
            //    }
            //};

            //Now In Regions when we hover we can see "Domain Model Region". So we're sending Domain Modle to client which is not a good practice;
            //Get Data from Database -- Domain Models here
            //var regions = DbContext.Regions.ToList();
            //Making this method async
            //Entity Framework provides Async functionality to all the methods
            var regions = await DbContext.Regions.ToListAsync();

            //Map Domain Models to DTO's
            var regionsDto = new List<RegionDto>();
            foreach (var region in regions)
            {
                regionsDto.Add(new RegionDto
                {
                    Id = region.Id,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl,
                    Name = region.Name
                }); ;

            }

            //Return DTOs
            return Ok(regionsDto);
        }

        //Get REgion by ID
        [HttpGet]
        [Route("{id:Guid}")] //Type not required to pass
        public async Task<IActionResult> GetById(Guid id) //([FromRoute] Guid id) Its working without FromRoute as well.not int--got error bc of this
        {
            //var region = DbContext.Regions.Find(id);
            var region = await DbContext.Regions.FirstOrDefaultAsync(x => x.Id == id); //It will work if we're searching with Name or something else.
            if (region == null)
            {
                return NotFound();
            }

            return Ok(region);
        }

        //POST : same route as above, but when user selects POST, it will come here
        //In Post method we have FromBody and giving type what is coming.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionDto addRegionRequestDTO)
        {
            //Map or Convert DTO to Domain Model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl
            };

            //Use Domain Model to create Region

            await DbContext.Regions.AddAsync(regionDomainModel);
            await DbContext.SaveChangesAsync(); //This line is important. Else it wont save data in database. Here only it will save data.

            //Map Domain Model back to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            //For post 201 code should send. And we are sending the created Database data
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        //PUT
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateRegionDto regionData)
        {
            var regionDomainModel = await DbContext.Regions.FindAsync(id); //So here we're directly get the data so, if we update on this one it will directly update in Database
            if(regionDomainModel == null)
            {
                return NotFound();
            }
            //Map DTO to Domain Model
            regionDomainModel.Code = regionData.Code;
            regionDomainModel.Name = regionData.Name;
            regionDomainModel.RegionImageUrl = regionData.RegionImageUrl;

            await DbContext.SaveChangesAsync();

            //Convert Domain Model to DTO
            var regionDTO = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDTO);
        }

        //DELETE
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await DbContext.Regions.FindAsync(id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }
            DbContext.Regions.Remove(regionDomainModel); //Remove doesn't have async version.
            await DbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
 */