using Auth.Auth.Interfaces;
using Auth.Auth.Models;
using Auth.Auth.Services;
using Auth.Mongo.Models;
using Auth.Mongo.Services;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Auth.Controllers
{
    [Route("authorization")]
    [ApiController]
    public class AuthorizationController : Controller
    {
        private ITokenGenerator _tokenGenerator;
        private IEncryptionHandler _encryptionHandler;
        private MongoUsersService _mongoUsersService;
        public AuthorizationController(ITokenGenerator tokenGenerator, IEncryptionHandler encryptionHandler, MongoUsersService mongoUsersService)
        {
            _tokenGenerator = tokenGenerator;
            _mongoUsersService = mongoUsersService;
            _encryptionHandler = encryptionHandler;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserModel user)
        {
            bool userAuthenticated = await _mongoUsersService.AuthenticateUser(
                user.Username,
                _encryptionHandler.Encrypt(Encoding.UTF8.GetBytes(user.Password))
                );
            if (userAuthenticated)
            {
                return Ok(JsonConvert.SerializeObject(_tokenGenerator.GenerateToken()));
            }
            else
            {
                return Unauthorized("Username or password are incorrect");
            }
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] UserModel user)
        {
            bool insertSuccessful = await _mongoUsersService.InsertAsync(new User()
            {
                Username = user.Username,
                Password = _encryptionHandler.Encrypt(Encoding.UTF8.GetBytes(user.Password))
            });
            if (insertSuccessful)
            {
                return Ok();
            }
            else
            {
                return Conflict("User with username " + user.Username + " already exists");
            }
        }
    }
}
