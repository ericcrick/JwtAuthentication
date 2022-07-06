using Microsoft.AspNetCore.Mvc;
using JwtAuthentication.Dtos;
using JwtAuthentication.Repositories;
using JwtAuthentication.Models;
using JwtAuthentication.Helpers;
using JwtAuthentication.Filters;
using Microsoft.AspNetCore.Authorization;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenManager _itokenManager;
        private readonly IPasswordManager _passwordManager;
        public AuthController(IUserRepository userRepository, ITokenManager tokenManager, IPasswordManager passwordManager)
        {
            _userRepository = userRepository;
            _itokenManager =tokenManager;
            _passwordManager = passwordManager;
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
                    Password = _passwordManager.HashPassword(userDto.Password)
                };
                await _userRepository.Create(user);
                return CreatedAtAction("New user created", user);
            }
            return BadRequest("Invalid payload");
        }

        [Authorize]
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
        [HttpPost("authenticate/user")]
        public async Task<ActionResult<string>> LogInUser(LogInUserDto logInUserDto)
        {
            var user = await _userRepository.GetByEmail(logInUserDto.Email);
            if(user != null){
                // verify password
                var verifyPassword = _passwordManager.VerifyUserPassword(logInUserDto.Password, user.Password);
                if(verifyPassword){
                    return Ok(new {
                        Token = _itokenManager.GenerateToken(user)
                    });
                }
                return BadRequest( new {
                    message = "Invalid Credentials"
                });
            }
            return BadRequest( new {
                message = "Invalid Credentials"
            });
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