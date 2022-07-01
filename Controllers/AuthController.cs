using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtAuthentication.Dtos;
using Microsoft.AspNetCore.Authorization;
using JwtAuthentication.Repositories;
using JwtAuthentication.Models;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        public AuthController(IConfiguration configuration, IUserRepository userRepository)
        {
            // injecting configuration
            _configuration = configuration;
            _userRepository = userRepository;
        }
        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser(CreateUserDto userDto){
            var user = new User(){
                Username = userDto.Username,
                Email = userDto.Email,
                Password = userDto.Password
            };
            await _userRepository.Create(user);
            return user;
        }
        [HttpPost("login")]
        public ActionResult LoginUser(CreateUserDto user){
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
        [Authorize]
        [HttpGet("home")]
        public ActionResult<string> GetHome(){
            return "Hello";
        }

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