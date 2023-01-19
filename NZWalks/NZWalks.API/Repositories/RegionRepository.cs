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

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await nZWalksDbcontext.AddAsync(region);
            await nZWalksDbcontext.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nZWalksDbcontext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            var region = nZWalksDbcontext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            return await region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = nZWalksDbcontext.Regions.FirstOrDefault(x => x.Id == id);
            if (region == null)
                return null;

            //Delete the region 
            nZWalksDbcontext.Regions.Remove(region);
            await nZWalksDbcontext.SaveChangesAsync();
            return region;

        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var Existingregion = await nZWalksDbcontext.Regions.FirstOrDefaultAsync(x => x.Id == id);  

            if(Existingregion == null)
            {
                return null;    
            }

            Existingregion.Code = region.Code;
            Existingregion.Name = region.Name;
            Existingregion.Area = region.Area;
            Existingregion.Lat = region.Lat;
            Existingregion.Long = region.Long;
            Existingregion.Population = region.Population;

            await nZWalksDbcontext.SaveChangesAsync();
            return Existingregion;  
        }
    }
}
