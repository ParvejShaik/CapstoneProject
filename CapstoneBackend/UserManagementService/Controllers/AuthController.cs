using Microsoft.AspNetCore.Mvc;
using UserManagementService.Data;
using UserManagementService.Models;
using UserManagementService.DTO;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UserManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string _jwtKey;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _jwtKey = configuration["Jwt:Key"];
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Authenticate([FromBody] LoginUserDto userObj)
        {
            if (userObj == null)
            {
                Console.WriteLine("User object is null");
                return BadRequest(new { Message = "User object is null" });
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userObj.Email);
            if (user == null)
            {
                Console.WriteLine($"User not found for email: {userObj.Email}");
                return NotFound(new { Message = "User Not Found" });
            }
            if (userObj.Password != user.Password)
            {
                Console.WriteLine("Password is incorrect");
                return BadRequest(new { Message = "Password is incorrect" });
            }
            if (userObj.Role!= user.Role)
            {
                Console.WriteLine("Select Appropriate Role");
                return BadRequest(new { Message = "Role is incorrect" });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Token = tokenString,
                Username = user.UserName,
                Role = user.Role
            });
        }

        
        [HttpGet("{username}")]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }





        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto userDto)
        {
            if (userDto == null)
                return BadRequest(new { Message = "User object is null" });

            if (await CheckUserNameExistAsync(userDto.UserName))
                return BadRequest(new { Message = "User Name already exists" });

            if (await CheckEmailExistAsync(userDto.Email))
                return BadRequest(new { Message = "Email Id already exists" });

            if (userDto.Role != "User")
            {
                return BadRequest(new { Message = "Role must be selected as 'User'" });
            }

            var passStrengthMsg = CheckPasswordStrength(userDto.Password);
            if (!string.IsNullOrEmpty(passStrengthMsg))
                return BadRequest(new { Message = passStrengthMsg });

            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Role = userDto.Role,
                Password = (userDto.Password),
                PhoneNumber = userDto.PhoneNumber, 
           
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User Registered" });
        }


        [HttpPut("{username}")]
        public async Task<IActionResult> UpdateUser(string username, [FromBody] UpdateUserDto updateUserDto)
        {
            if (updateUserDto == null)
                return BadRequest(new { Message = "Request object is null" });

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if (user == null)
                return NotFound(new { Message = "User not found" });

           
            if (!string.IsNullOrEmpty(updateUserDto.UserName))
            {
                user.UserName = updateUserDto.UserName;
            }

            if (!string.IsNullOrEmpty(updateUserDto.PhoneNumber))
            {
                user.PhoneNumber = updateUserDto.PhoneNumber;
            }
            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                user.Password= updateUserDto.Password;
            }

            if (!string.IsNullOrEmpty(updateUserDto.Role))
            {
                user.Role = updateUserDto.Role;
            }
            if (!string.IsNullOrEmpty(updateUserDto.Email))
            {
                user.Email= updateUserDto.Email;
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User details updated successfully" });
        }




        private async Task<bool> CheckEmailExistAsync(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        private async Task<bool> CheckUserNameExistAsync(string userName)
        {
            return await _context.Users.AnyAsync(x => x.UserName == userName);
        }

        private string CheckPasswordStrength(string password)
        {
            if (password.Length < 8)
                return "Password must be at least 8 characters long";

            if (!Regex.IsMatch(password, @"[A-Z]"))
                return "Password must contain at least one uppercase letter";

            if (!Regex.IsMatch(password, @"[a-z]"))
                return "Password must contain at least one lowercase letter";

            if (!Regex.IsMatch(password, @"[0-9]"))
                return "Password must contain at least one number";

            if (!Regex.IsMatch(password, @"[\W_]"))
                return "Password must contain at least one special character";

            return null;
        }
    }
}
