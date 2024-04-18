using AuthApi.AuthApi.Interfaces;
using AuthApi.AuthApi.Models.Dtos;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthApi.AuthApi.Controllers
{
    [Route("token")]
    [ApiController]
    public class TokenController: Controller
    {
        private readonly IRefreshTokenHandler _refreshTokenHandler;
        private readonly ITokenService _tokenService;
        public TokenController(
            IRefreshTokenHandler refreshTokenHandler,
            ITokenService tokenService) 
        {
            _refreshTokenHandler = refreshTokenHandler;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] Tokens tokens)
        {
            ClaimsPrincipal principals = _tokenService.GetPrincipalFromExpiredToken(tokens.AccessToken);
            string username = principals.Identity.Name;
            if (!_refreshTokenHandler.IsValid(username, tokens.RefreshToken))
            {
                return Unauthorized("Invalid Refresh Token");
            }

            string newAccessToken = _tokenService.GenerateAccessToken(principals.Claims);

            return Ok(newAccessToken);
        }

        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;
            bool deletedSuccesfully = _refreshTokenHandler.DeleteRefreshToken(username);
            if (deletedSuccesfully)
            {
                return Ok();
            }
            else
            {
                return BadRequest("User already revoked");
            }
        }
    }
}
