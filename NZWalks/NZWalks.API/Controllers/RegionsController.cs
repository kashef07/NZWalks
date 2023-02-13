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
            //Validate the Request
            //if (!ValidateAddRegionAsync(addRegionRequest))
            //{
            //    return BadRequest(ModelState);
            //}

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
            //Validate the Request
            //if (!ValidateUpdateRegionAsync(UpdateRegionRequest))
            //{
            //    return BadRequest(ModelState);
            //}

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

        #region Private methods

        private bool ValidateAddRegionAsync(Model.DTO.AddRegionRequest addRegionRequest)
        {
            if(addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest),
                    $"Add region data is required");
                return false;
            }

            if(string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code), $"{nameof(addRegionRequest.Code)} cannot be null or empty");
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name), $"{nameof(addRegionRequest.Name)} cannot be null or empty");
            }

            if(addRegionRequest.Area <=0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area), $"{nameof(addRegionRequest.Area)} cannot be less than or equal to zero");
            }

/*            if (addRegionRequest.Lat <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Lat), $"{nameof(addRegionRequest.Lat)}cannot be less than or equal to zero");
            }

            if (addRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Long), $"{nameof(addRegionRequest.Long)}cannot be less than or equal to zero");
            }*/

            if (addRegionRequest.Population <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population), $"{nameof(addRegionRequest.Population)} cannot be less than zero");
            }

            if(ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;

        }

        private bool ValidateUpdateRegionAsync(Model.DTO.UpdateRegionRequest UpdateRegionRequest)
        {
            if (UpdateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(UpdateRegionRequest),
                    $"Add region data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(UpdateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(UpdateRegionRequest.Code), $"{nameof(UpdateRegionRequest.Code)} cannot be null or empty");
            }

            if (string.IsNullOrWhiteSpace(UpdateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(UpdateRegionRequest.Name), $"{nameof(UpdateRegionRequest.Name)} cannot be null or empty");
            }

            if (UpdateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(UpdateRegionRequest.Area), $"{nameof(UpdateRegionRequest.Area)} cannot be less than or equal to zero");
            }

            if (UpdateRegionRequest.Population <= 0)
            {
                ModelState.AddModelError(nameof(UpdateRegionRequest.Population), $"{nameof(UpdateRegionRequest.Population)} cannot be less than zero");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
