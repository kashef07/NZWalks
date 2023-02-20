using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Model.DTO;
using NZWalks.API.Repositories;
using System.Data;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultiesRepository _walkDifficultiesRepository;
        private readonly IMapper _mapper;

        public WalkDifficultiesController(IWalkDifficultiesRepository walkDifficultiesRepository, IMapper mapper)
        {
            _walkDifficultiesRepository = walkDifficultiesRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var WalkDifficultyDomain = await _walkDifficultiesRepository.GetAllAsync();
            var WalkDifficultyDTO = _mapper.Map<List<Model.DTO.WalkDifficulty>>(WalkDifficultyDomain);
            return Ok(WalkDifficultyDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficulties")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetWalkDifficulties(Guid id)
        {
            var walkDifficulty = await _walkDifficultiesRepository.GetAsync(id);
            if (walkDifficulty == null)
            {
                return null;
            }
            var WalkDifficultyDTO = _mapper.Map<Model.DTO.WalkDifficulty>(walkDifficulty);
            return Ok(WalkDifficultyDTO);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkDifficulties(Model.DTO.AddWalkDifficultyRepository addWalkDifficulty)
        {
            // // validate incoming request
            //if(!await ValidateAddWalkDifficulties(addWalkDifficulty))
            //{
            //    return BadRequest(ModelState);
            //}

            var WalkDifficultyDomain = new Model.Domain.WalkDifficulty
            {
                Code = addWalkDifficulty.Code
            };

            WalkDifficultyDomain = await _walkDifficultiesRepository.AddAsync(WalkDifficultyDomain);

            var WalkDifficultyDTO = _mapper.Map<Model.DTO.WalkDifficulty>(WalkDifficultyDomain);
            return CreatedAtAction(nameof(GetWalkDifficulties), new { id = WalkDifficultyDTO.Id }, WalkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkDifficulties(Guid id, Model.DTO.UpdateWalkDifficultiesRequest updateWalkDifficultiesRequest)
        {
            //Validate the Request
            //if (!await ValdateUpdateWalkDifficulties(updateWalkDifficultiesRequest))
            //{
            //    return BadRequest(ModelState);
            //}

            var WalkDifficultyDomain = new Model.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultiesRequest.Code
            };

            WalkDifficultyDomain = await _walkDifficultiesRepository.UpdateAsync(id, WalkDifficultyDomain);

            if (WalkDifficultyDomain == null)
            {
                return null;
            }

            var WalkDifficultyDTO = _mapper.Map<Model.DTO.WalkDifficulty>(WalkDifficultyDomain);
            return Ok(WalkDifficultyDTO);
            }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteDifficulties(Guid id)
        {
            var WalkDifficuty = await _walkDifficultiesRepository.DeleteAsync(id);

            if(WalkDifficuty == null)
            {
                return NotFound();
            }

            var WalkDificultyDTO = _mapper.Map<Model.DTO.WalkDifficulty>(WalkDifficuty);
            return Ok(WalkDificultyDTO);
    }

        #region Private Methods
        private async Task<bool> ValidateAddWalkDifficulties(Model.DTO.AddWalkDifficultyRepository addWalkDifficulty)
        {
            if(addWalkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficulty),$"{nameof(addWalkDifficulty)} is required");
            }

            if(string.IsNullOrWhiteSpace(addWalkDifficulty.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficulty.Code),$"{nameof(addWalkDifficulty.Code)} is required");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> ValdateUpdateWalkDifficulties(Model.DTO.UpdateWalkDifficultiesRequest updateWalkDifficultiesRequest)
        {
            if(updateWalkDifficultiesRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultiesRequest), $"{nameof(updateWalkDifficultiesRequest)} cannot be empty");
                return false;
            }

            if(string.IsNullOrWhiteSpace(updateWalkDifficultiesRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultiesRequest.Code), $"{nameof(updateWalkDifficultiesRequest)} is required");  
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
