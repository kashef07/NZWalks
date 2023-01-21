using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Model.Domain;
using System.Reflection.Metadata.Ecma335;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDBContext nZWalksDbcontext;

        public WalkRepository(NZWalksDBContext nZWalksDbcontext)
        {
            this.nZWalksDbcontext = nZWalksDbcontext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();
           await nZWalksDbcontext.Walks.AddAsync(walk);
            await nZWalksDbcontext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
           var ExistingWalk =  await nZWalksDbcontext.Walks.FindAsync(id);

            if (ExistingWalk == null)
            {
                return null;
            }

            nZWalksDbcontext.Walks.Remove(ExistingWalk);
            await nZWalksDbcontext.SaveChangesAsync();
            return ExistingWalk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await 
                nZWalksDbcontext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();  
        }
            
        public async Task<Walk> GetAsync(Guid id)
        {
            return await nZWalksDbcontext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);  
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var ExistingWalk = await nZWalksDbcontext.Walks.FindAsync(id);
            if (ExistingWalk != null)
            {
                ExistingWalk.Length = walk.Length;
                ExistingWalk.Name = walk.Name;
                ExistingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                ExistingWalk.RegionId = walk.RegionId;
                await nZWalksDbcontext.SaveChangesAsync();
                return ExistingWalk;
            }
            return null;
        }
    }
}
