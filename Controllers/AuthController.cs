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
        // register new user
        [HttpPost("register")]
        public async Task<ActionResult<ReadUserDto>> RegisterUser(CreateUserDto userDto){
            var user = new User(){
                Username = userDto.Username,
                Email = userDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };
            await _userRepository.Create(user);
            return CreatedAtAction("New user created",user);
        }
        [HttpGet("find/{id}")]
        public async Task<ActionResult<ReadUserDto>> FindById(int id){
            var user = await _userRepository.FindById(id);
            if(user == null) return NotFound("User not found");
            return new ReadUserDto(){
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }
        // login user
        [HttpPost("login")]
        public async Task<ActionResult<string>> LogInUser(LogInUserDto logInUserDto){
            return $"Login{logInUserDto}";
        }

        // private JwtSecurityToken GetToken( List<Claim> authClaims){
        //     var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        //     var token = new JwtSecurityToken(
        //         issuer: _configuration["JWT:Issuer"],
        //         audience: _configuration["JWT:Audience"],
        //         expires: DateTime.UtcNow.AddHours(24),
        //         claims: authClaims,
        //         signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
        //     );
        //     return token;
        // }
    }
}