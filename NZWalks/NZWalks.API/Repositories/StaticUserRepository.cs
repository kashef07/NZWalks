using NZWalks.API.Model.Domain;

namespace NZWalks.API.Repositories
{
    public class StaticUserRepository : IUserRepository
    {
        private List<User> Users = new List<User>()
        {
        //    new User()
        //    {
        //        FirstName ="ReadOnly",
        //        LastName = "User",
        //        EmailAddress = "Readonly@user.com",
        //        Id = Guid.NewGuid(),
        //        Username = "Readonly@User.com",
        //        password = "Readonly@user",
        //        Roles = new List<string> {"Reader"}
        //    },

        //     new User()
        //     {
        //        FirstName ="ReadWrite",
        //        LastName = "User",
        //        EmailAddress = "ReadWrite@user.com",
        //        Id = Guid.NewGuid(),
        //        Username = "ReadWrite@User.com",
        //        password = "ReadWrite@user",
        //        Roles = new List<string> {"Reader", "Writer"}
        //    },

        };

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user =  Users.Find(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) && x.password == password);
            return user;
        }
    }
}
