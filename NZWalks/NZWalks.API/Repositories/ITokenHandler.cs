using NZWalks.API.Model.Domain;

namespace NZWalks.API.Repositories
{
    public interface ITokenHandler
    {
        Task<string> CreateTokenAsync(User user);
    }
}
