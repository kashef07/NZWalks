using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Model.Domain;

namespace NZWalks.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NZWalksDBContext _nZWalksDBContext;

        public UserRepository(NZWalksDBContext nZWalksDBContext)
        {
            _nZWalksDBContext = nZWalksDBContext;
        }
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _nZWalksDBContext.Users.FirstOrDefaultAsync(x=> x.Username == username.ToLower() && x.password == password);

            if(user == null)
            {
                return null;
            }

            var userroles = await _nZWalksDBContext.User_Roles.Where(x => x.UserId == user.Id).ToListAsync();
            if(userroles.Any())
            {
                user.Roles = new List<string>();
                foreach (var userrole in userroles)
                {
                    var role =await _nZWalksDBContext.Roles.FirstOrDefaultAsync(x => x.Id == userrole.RoleId);  
                    if(role != null)
                    {
                        user.Roles.Add(role.Name);
                    }
                }
            }

            user.password = null;
            return user;
        }
    }
}
