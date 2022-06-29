using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtAuthentication.Dtos;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            // injecting configuration
            _configuration = configuration;
        }

    [HttpPost("login")]
    public ActionResult LoginUser(UserDto user){
        var authClaims = new List<Claim>(){
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("Id", "1"),
        };
        var token = this.GetToken(authClaims);
        return Ok(
            new {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            }
        );
    }
        // create jwt token
        private JwtSecurityToken GetToken( List<Claim> authClaims){
            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddHours(24),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }
    }
}