using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Model.Domain;
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
            var regions = await regionRepository.GetAllAsunc();
            
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
    } 
}
