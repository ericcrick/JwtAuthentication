using Microsoft.AspNetCore.Mvc;
using JwtAuthentication.Dtos;
using JwtAuthentication.Repositories;
using JwtAuthentication.Models;
using JwtAuthentication.Helpers;
using JwtAuthentication.Filters;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        public AuthController(IUserRepository userRepository, IJwtService jwtService)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
        }
        // register new user
        [HttpPost("register")]
        public async Task<ActionResult<ReadUserDto>> RegisterUser(CreateUserDto userDto)
        {
            if(!string.IsNullOrWhiteSpace(userDto.Email)
            && !string.IsNullOrWhiteSpace(userDto.Username)
            && !string.IsNullOrWhiteSpace(userDto.Password)){
                var user = new User()
                {
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
                };
                await _userRepository.Create(user);
                return CreatedAtAction("New user created", user);
            }
            return BadRequest("Invalid payload");
        }

        [DebugResourceFilter]
        [HttpGet("find/{id}")]
        public async Task<ActionResult<ReadUserDto>> FindById(int id)
        {
            var user = await _userRepository.FindById(id);
            if (user == null) return NotFound("User not found");
            return new ReadUserDto()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }
        // login user
        [HttpPost("login")]
        public async Task<ActionResult<string>> LogInUser(LogInUserDto logInUserDto)
        {
            var user = await _userRepository.GetByEmail(logInUserDto.Email);
            if (user == null) return BadRequest(new { message = "Invalid Credentials" });
            if (!BCrypt.Net.BCrypt.Verify(logInUserDto.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }
            var token = _jwtService.GenerateToken(user.Id);
            Response.Cookies.Append("jwt", token, new CookieOptions{
                HttpOnly = true
            });
            return Ok(new {
                message = "Login Successfully"
            });
        }

        [HttpGet("user")]
        public async Task<ActionResult<ReadUserDto>> UserProfile(){
            try
            {
                var jwt = Request.Cookies["jwt"];
                var validateToken = _jwtService.VerifyToken(jwt!);
                int userId = int.Parse(validateToken.Issuer);
                var user = await _userRepository.FindById(userId);
                var authenticatedUser = new ReadUserDto(){
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                };
                return Ok(authenticatedUser);
            }
            catch (Exception )
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public IActionResult LogOut(){
            Response.Cookies.Delete("jwt");
            return Ok(
                new { message = "Success"}
            );
        }
    }
}