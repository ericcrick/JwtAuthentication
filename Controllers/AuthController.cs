using Microsoft.AspNetCore.Mvc;
using JwtAuthentication.Dtos;
using JwtAuthentication.Repositories;
using JwtAuthentication.Models;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public AuthController(IUserRepository userRepository)
        {
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
        public async Task<ActionResult<ReadUserDto>> LogInUser(LogInUserDto logInUserDto){
            var user = await _userRepository.GetByEmail(logInUserDto.Email);
            if(user == null) return BadRequest( new { message = "Invalid Credentials"});
            if(!BCrypt.Net.BCrypt.Verify(logInUserDto.Password, user.Password)){
                return BadRequest( new { message = "Invalid Credentials"});
            }
            return Ok(user);
        }

    }
}