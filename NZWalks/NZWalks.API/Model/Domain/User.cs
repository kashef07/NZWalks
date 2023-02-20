using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Model.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public List<string> Roles { get; set; } 
        
        //Navigation Property

        public List<User_Role> UserRoles { get; set; }
    }
}
