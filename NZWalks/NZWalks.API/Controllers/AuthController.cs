using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository UserRepository;
        private readonly ITokenHandler _tokenHandler;

        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
        {
            UserRepository = userRepository;
            _tokenHandler = tokenHandler;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Model.DTO.LoginRequest loginRequest)
        {
            var user = await UserRepository.AuthenticateAsync(loginRequest.username, loginRequest.password);

            if(user != null)
            {
                //Generate a JWT Token 
                var token = await _tokenHandler.CreateTokenAsync(user);
                return Ok (token);
            }

            return BadRequest("Username or Password is incorrect");
        }
    }
}
