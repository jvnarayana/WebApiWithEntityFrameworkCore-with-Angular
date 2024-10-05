using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.DTO;
using WebApplication1.Entities;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    public AuthController(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }
    // GET
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserDTO userDto)
    {
        var existingUser = _userRepository.GetUserByUserNameAsync(userDto.UserName!);
        if (existingUser!= null && existingUser.Id > 1)
        {
            return BadRequest("user already exists");
        }

        var passwordEncrypt = BCrypt.Net.BCrypt.HashPassword(userDto?.Password);
        var user = new User { UserName = userDto?.UserName, PasswordHash = passwordEncrypt };
        await _userRepository.AddUserAsync(user);
        return Ok("User Registered Successfully! ");

    }

        [HttpGet("Login")]
        public async Task<IActionResult> Login([FromQuery] string userName, [FromQuery] string password)
        {
            var user = await _userRepository.GetUserByUserNameAsync(userName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return Unauthorized("Invalid Creds");
            }

            var jwtToken = GenerateJWTToken(user);
            return Ok( new { token = jwtToken});

        }

    private string GenerateJWTToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:key"]);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
           Subject = new ClaimsIdentity(new[]
           {
             new Claim(ClaimTypes.Name, user.UserName)   
           }),
           Expires = DateTime.UtcNow.AddDays(1),
           SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}