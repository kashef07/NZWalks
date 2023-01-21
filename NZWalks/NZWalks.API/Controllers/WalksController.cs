using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Model.Domain;
using NZWalks.API.Model.DTO;
using NZWalks.API.Repositories;
using System.Security.Cryptography.X509Certificates;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            // Fetch data from database - domain walks
            var WalksDomain = await _walkRepository.GetAllAsync();

            // Convert domain walks to DTO Walks
            var WalksDTO = _mapper.Map<List<Model.DTO.Walk>>(WalksDomain);

            // Return response
            return Ok(WalksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult>GetWalkAsync(Guid id)
        {
            var WalkDomain = await _walkRepository.GetAsync(id);
            var WalksDTO = _mapper.Map<Model.DTO.Walk>(WalkDomain);   
            return Ok(WalksDTO);
        }

        [HttpPost]
        public async Task<IActionResult>AddWalkAsync([FromBody]Model.DTO.AddWalkRequest addWalkRequest)
        {
            var walkDomain = new Model.Domain.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId,
            };

            walkDomain = await _walkRepository.AddAsync(walkDomain);

            var walkDTO = new Model.DTO.Walk()
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId,
            };
            return CreatedAtAction(nameof(GetWalkAsync), new {id = walkDTO.Id},walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Model.DTO.UpdateWalkRequest updateWalkRequest)
        {
            var WalkDomain = new Model.Domain.Walk()
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId,
            };

            WalkDomain = await _walkRepository.UpdateAsync(id, WalkDomain);

            if(WalkDomain == null)
            {
                return NotFound();
            }

            var WalkDTO = new Model.DTO.Walk()
            {
                Id = WalkDomain.Id,
                Length = WalkDomain.Length,
                Name = WalkDomain.Name,
                RegionId = WalkDomain.RegionId,
                WalkDifficultyId = WalkDomain.WalkDifficultyId,
            };

            return Ok(WalkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult>DeleteWalk(Guid id)
        {
            var WalkDomain = await _walkRepository.DeleteAsync(id);
            if (WalkDomain == null)
            {
                return NotFound();
            }

            var WalkDTO = new Model.DTO.Walk()
            {
                Id = WalkDomain.Id,
                Length = WalkDomain.Length,
                Name = WalkDomain.Name,
                RegionId = WalkDomain.RegionId,
                WalkDifficultyId = WalkDomain.WalkDifficultyId,
            };
            return Ok(WalkDTO);
        }
    }
}
