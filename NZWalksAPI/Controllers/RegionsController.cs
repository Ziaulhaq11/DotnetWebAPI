using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;

namespace NZWalksAPI.Controllers
{
    //https://localhost:portNumber/api/Regions
    [Route("api/[controller]")]
    [ApiController] //will automatically tell that its for API use. And it valdiates modal state and gives 400 response to the caller
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext DbContext;
        public RegionsController(NZWalksDbContext dbContext)
        {
            this.DbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            /*var regions = new List<Region>
            {
                new Region
                {
                    Id = Guid.NewGuid(),
                    Name = "Auckland Region",
                    Code = "AKL",
                    RegionImageUrl = "https://images.pexels.com/photos/17963791/pexels-photo-17963791.jpeg?cs=srgb&dl=pexels-wladislawa-schr%C3%B6der-17963791.jpg&fm=jpg&w=4000&h=6000&_gl=1*1qhxymr*_ga*MTQ3OTQ3MDE4OC4xNjk5MzY4MjQx*_ga_8JE65Q40S6*MTY5OTM2ODI0My4xLjAuMTY5OTM2ODI0My4wLjAuMA"
                },
                new Region
                {
                    Id = Guid.NewGuid(),
                    Name = "Wellington Region",
                    Code = "WLG",
                    RegionImageUrl = "https://images.pexels.com/photos/17963791/pexels-photo-17963791.jpeg?cs=srgb&dl=pexels-wladislawa-schr%C3%B6der-17963791.jpg&fm=jpg&w=4000&h=6000&_gl=1*1qhxymr*_ga*MTQ3OTQ3MDE4OC4xNjk5MzY4MjQx*_ga_8JE65Q40S6*MTY5OTM2ODI0My4xLjAuMTY5OTM2ODI0My4wLjAuMA"
                }
            };*/

            //Now In Regions when we hover we can see "Domain Model Region". So we're sending Domain Modle to client which is not a good practice;
            //Get Data from Database -- Domain Models here
            var regions = DbContext.Regions.ToList();

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
        public IActionResult GetById(Guid id) //([FromRoute] Guid id) Its working without FromRoute as well.not int--got error bc of this
        {
            //var region = DbContext.Regions.Find(id);
            var region = DbContext.Regions.FirstOrDefault(x => x.Id == id); //It will work if we're searching with Name or something else.
            if (region == null)
            {
                return NotFound();
            }

            return Ok(region);
        }

        //POST : same route as above, but when user selects POST, it will come here
        //In Post method we have FromBody and giving type what is coming.
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionDto addRegionRequestDTO)
        {
            //Map or Convert DTO to Domain Model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl
            };

            //Use Domain Model to create Region

            DbContext.Regions.Add(regionDomainModel);
            DbContext.SaveChanges(); //This line is important. Else it wont save data in database. Here only it will save data.

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
        public IActionResult Update([FromRoute] Guid id,[FromBody] UpdateRegionDto regionData)
        {
            var regionDomainModel = DbContext.Regions.Find(id); //So here we're directly get the data so, if we update on this one it will directly update in Database
            if(regionDomainModel == null)
            {
                return NotFound();
            }
            //Map DTO to Domain Model
            regionDomainModel.Code = regionData.Code;
            regionDomainModel.Name = regionData.Name;
            regionDomainModel.RegionImageUrl = regionData.RegionImageUrl;

            DbContext.SaveChanges();

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
        public IActionResult Delete([FromRoute] Guid id)
        {
            var regionDomainModel = DbContext.Regions.Find(id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }
            DbContext.Regions.Remove(regionDomainModel);
            DbContext.SaveChanges();
            return Ok();
        }
    }
}
