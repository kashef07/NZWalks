using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Model.Domain;
using NZWalks.API.Model.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionReposiitory regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionReposiitory regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await regionRepository.GetAllAsync();
            
            //return DTO regions

        /*    var regionsDTO = new List<Model.DTO.Region>();
            regions.ToList().ForEach(region =>
            {
                var regionDTO = new Model.DTO.Region()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Area = region.Area,
                    Lat = region.Lat,
                    Long = region.Long,
                    Population = region.Population,
                };

                regionsDTO.Add(regionDTO);
            });*/

            var regionsDTO = _mapper.Map<List<Model.DTO.Region>>(regions);

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if (region == null)
            {
                return NotFound();  
            }
            var regionDTO = _mapper.Map<Model.DTO.Region>(region);
            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Model.DTO.AddRegionRequest addRegionRequest)
        {
            // Request(DTO) to Domain model
            var region = new Model.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population,
            };

            // Pass details to Repository
            region = await regionRepository.AddAsync(region);

            // Convert back to DTO
            var regionDTO = new Model.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,
            };
            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            // Get region from database
            var region = await regionRepository.DeleteAsync(id);

            // If null NotFound
            if (region == null)
            {
                return NotFound();
            }

            // Convert response back to DTO
            
            var regionDTO = new Model.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,
            };
            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id , [FromBody] Model.DTO.UpdateRegionRequest UpdateRegionRequest)
        {
            // Convert DTO to Domain model
            var region = new Model.Domain.Region()
            {
                Code = UpdateRegionRequest.Code,
                Area = UpdateRegionRequest.Area,
                Lat = UpdateRegionRequest.Lat,
                Long = UpdateRegionRequest.Long,
                Name = UpdateRegionRequest.Name,
                Population = UpdateRegionRequest.Population,
            };

            // Update Region using repository
            region = await regionRepository.UpdateAsync(id, region);

            //If null then Not Found
            if (region ==null)
            {
                return NotFound();
            }

            //Convert Domain to DTO
            var regionDTO = new Model.DTO.Region()
            {
                Id= region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };

            return Ok(regionDTO);
        }
    } 
}
