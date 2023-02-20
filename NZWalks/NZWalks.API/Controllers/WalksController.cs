using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Model.Domain;
using NZWalks.API.Model.DTO;
using NZWalks.API.Repositories;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;
        private readonly IRegionReposiitory _regionRepository;
        private readonly IWalkDifficultiesRepository _walkDifficultiesRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionReposiitory regionRepository, IWalkDifficultiesRepository walkDifficultiesRepository)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
            _regionRepository = regionRepository;
            _walkDifficultiesRepository = walkDifficultiesRepository;
        }

        [HttpGet]
        [Authorize(Roles = "reader")]
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
        [Authorize(Roles = "reader")]
        public async Task<IActionResult>GetWalkAsync(Guid id)
        {
            var WalkDomain = await _walkRepository.GetAsync(id);
            var WalksDTO = _mapper.Map<Model.DTO.Walk>(WalkDomain);   
            return Ok(WalksDTO);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult>AddWalkAsync([FromBody]Model.DTO.AddWalkRequest addWalkRequest)
        {
            //Validate the Request
            if(!(await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }

            // Convert DTO to Domain Object
            var walkDomain = new Model.Domain.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId,
            };

            // Pass domain object to Repository to persist this
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
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Model.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Validate the Request
            if (!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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
        [Authorize(Roles = "writer")]
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

        #region private Method
        private async Task <bool> ValidateAddWalkAsync(Model.DTO.AddWalkRequest addWalkRequest)
        {
            if(addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest),$"{nameof(addWalkRequest)} cannot be empty");
                return false;
            }

            if(string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} is required");
            }

            if(addWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length),$"{nameof(addWalkRequest.Length)} should be greater than zero");
            }

            //var region = await _walkRepository.GetAsync(addWalkRequest.RegionId);
            var region = await _regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
            {
               ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is invalid");
            }

            //var walkdifficulty = await _walkRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            var walkdifficulty = await _walkDifficultiesRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkdifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid");
            }


            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;

        }

        private async Task<bool> ValidateUpdateWalkAsync(Model.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest), $"{nameof(updateWalkRequest)} cannot be empty");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} is required");
            }

            if (updateWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length), $"{nameof(updateWalkRequest.Length)} should be greater than zero");
            }

           // var region = await _walkRepository.GetAsync(updateWalkRequest.RegionId);
            var region = await _regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)} is invalid");
            }

            //var walkdifficulty = await _walkRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            var walkdifficulty = await _walkDifficultiesRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkdifficulty == null)
            {
                ModelState. AddModelError(nameof(updateWalkRequest.WalkDifficultyId), $"{nameof(updateWalkRequest.WalkDifficultyId)} is invalid");
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
