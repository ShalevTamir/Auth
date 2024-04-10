using Auth.Auth.Interfaces;
using Auth.Auth.Models;
using Auth.Auth.Services;
using Auth.Mongo.Models;
using Auth.Mongo.Services;
using AuthApi.AuthApi.Interfaces;
using AuthApi.AuthApi.Models.Dtos;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Auth.Controllers
{
    [Route("authorization")]
    [ApiController]
    public class AuthorizationController : Controller
    {
        private ITokenService _tokenService;
        private IEncryptionHandler _encryptionHandler;
        private IRefreshTokenHandler _refreshTokenHandler;
        private MongoUsersService _mongoUsersService;
        public AuthorizationController(
            ITokenService tokenService,
            IEncryptionHandler encryptionHandler,
            IRefreshTokenHandler refreshTokenHandler,
            MongoUsersService mongoUsersService)
        {
            _tokenService = tokenService;
            _mongoUsersService = mongoUsersService;
            _encryptionHandler = encryptionHandler;
            _refreshTokenHandler = refreshTokenHandler;
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
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                };
                var accessToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                _refreshTokenHandler.SaveRefreshToken(user.Username, refreshToken);

                return Ok(
                    new Tokens()
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    }
                );
            }
            return Unauthorized("Username or password are incorrect");
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
