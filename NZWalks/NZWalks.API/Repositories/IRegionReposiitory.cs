using NZWalks.API.Model.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionReposiitory
    {
        Task<IEnumerable<Region>> GetAllAsunc();
    }
}
