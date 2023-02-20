using NZWalks.API.Model.Domain;

namespace NZWalks.API.Repositories
{
    public interface IUserRepository
    {
        Task<User> AuthenticateAsync(string username, string password);
    }
}
