using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Model.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultiesRepository : IWalkDifficultiesRepository
    {
        private readonly NZWalksDBContext _nZWalksDBContext;

        public WalkDifficultiesRepository(NZWalksDBContext nZWalksDBContext)
        {
            _nZWalksDBContext = nZWalksDBContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await _nZWalksDBContext.AddAsync(walkDifficulty);
            await _nZWalksDBContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var ExistingWalkDifficulty = await _nZWalksDBContext.WalkDifficulty.FindAsync(id);
            if(ExistingWalkDifficulty !=null )
            {
                _nZWalksDBContext.WalkDifficulty.Remove(ExistingWalkDifficulty);
                await _nZWalksDBContext.SaveChangesAsync();
                return ExistingWalkDifficulty;
            }

            return null;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await _nZWalksDBContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await _nZWalksDBContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var ExistingWalkDifficulty = await _nZWalksDBContext.WalkDifficulty.FindAsync(id);
            if(ExistingWalkDifficulty == null)
            {
                return null;
            }
            ExistingWalkDifficulty.Code = walkDifficulty.Code;
            await _nZWalksDBContext.SaveChangesAsync();
            return ExistingWalkDifficulty;
        }
    }
}
