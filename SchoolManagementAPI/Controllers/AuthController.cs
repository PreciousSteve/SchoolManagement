using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.Domain.UserManagement;
using SchoolManagement.Dto;
using SchoolManagement.Mappers;
using SchoolManagement.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SchoolManagementDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(SchoolManagementDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register/student")]
        public async Task<ActionResult> RegisterStudent([FromBody] StudentCreateDto dto)
        {
            if (!ModelState.IsValid)
            {  
                return BadRequest(ModelState);
            }
                

            var existingStudent = await _context.Students.AnyAsync(s => s.Email == dto.Email);
            if (existingStudent)
            {
                return BadRequest(new { message = "This Email already exists in the system." });
            }

            var student = dto.ToEntity();

            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Student created successfully. ", StudentId = student.Id});
        }


        [HttpPost("register/teacher")]
        public async Task<ActionResult> RegisterTeacher([FromBody] TeacherCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var existingTeacher = await _context.Teachers.AnyAsync(t => t.Email == dto.Email);
            if (existingTeacher)
            {
                return BadRequest(new { message = "This Email already exists in the system." });
            }

            var teacher = dto.ToEntity();

            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Teacher created successfully", TeacherId = teacher.Id });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new { message = "Email and Password are required." });
            }

            var student = await _context.Students.FirstOrDefaultAsync(s => s.Email == loginRequest.Email);
            if (student != null && VerifyPassword(loginRequest.Password, student.PasswordHash!))
            {
                var token = GenerateJwtToken(student.Email, student.Id.ToString(), "Student");
                return Ok(new LoginResponseDto
                {
                    Token = token,
                    Email = student.Email,
                    Id = student.Id,
                    Role = "Student"
                });
            }

            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Email == loginRequest.Email);
            if (teacher != null && VerifyPassword(loginRequest.Password, teacher.PasswordHash!))
            {
                var token = GenerateJwtToken(teacher.Email, teacher.Id.ToString(), "Teacher");
                return Ok(new LoginResponseDto
                {
                    Token = token,
                    Email = teacher.Email,
                    Id = teacher.Id,
                    Role = "Teacher"
                });
            }

            return Unauthorized(new { message = "Invalid Email or Password." });
        }

        [HttpGet("me")]
        [Authorize]
        public ActionResult<CurrentUserDto> GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var email = identity.FindFirst(ClaimTypes.Email).Value;
            var role = identity.FindFirst(ClaimTypes.Role).Value;

            var currentUser = new CurrentUserDto
            {
                Id = int.Parse(userId),
                Email = email,
                Role = role
            };
            return Ok(currentUser);
        }

        private string GenerateJwtToken(string email, string userId, string role)
        {
           var claims = new[]
           {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
           };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Issuer"],
                audience: _configuration["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
            { 
                return false;
            }
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, storedHash);
            return isPasswordValid;
        }






    }
}
