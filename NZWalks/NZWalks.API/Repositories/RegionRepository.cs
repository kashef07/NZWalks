using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Model.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionReposiitory
    {
        private readonly NZWalksDBContext nZWalksDbcontext;

        public RegionRepository(NZWalksDBContext nZWalksDbcontext)
        {
            this.nZWalksDbcontext = nZWalksDbcontext;
        }
        public async Task<IEnumerable<Region>> GetAllAsunc()
        {
            return await nZWalksDbcontext.Regions.ToListAsync();
        }
    }
}
