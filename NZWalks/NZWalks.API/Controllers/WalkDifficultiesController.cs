using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Model.DTO;
using NZWalks.API.Repositories;

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
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var WalkDifficultyDomain = await _walkDifficultiesRepository.GetAllAsync();
            var WalkDifficultyDTO = _mapper.Map<List<Model.DTO.WalkDifficulty>>(WalkDifficultyDomain);
            return Ok(WalkDifficultyDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficulties")]
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
        public async Task<IActionResult> AddWalkDifficulties(Model.DTO.AddWalkDifficultyRepository addWalkDifficulty)
        {
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
        public async Task<IActionResult> UpdateWalkDifficulties(Guid id, UpdateWalkDifficultiesRequest updateWalkDifficultiesRequest)
        {
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
    }
}
