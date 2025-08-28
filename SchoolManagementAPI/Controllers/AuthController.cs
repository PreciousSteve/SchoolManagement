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
                return BadRequest(new { message = "Email and Password are Required" });
            }

            var student = await _context.Students.FirstOrDefaultAsync(em => em.Email == loginRequest.Email);

            if (student == null)
            {
                return Unauthorized("Invalid Email or Password");
            }
            if (!VerifyPassword(loginRequest.Password, student.PasswordHash!))
            {
                return Unauthorized("Invalid Email or Password");
            }

            var token = GenerateJwtToken(student);

            return Ok(new LoginResponseDto
            {
                Token = token,
                Email = student.Email,
                Id = student.Id
            });



        }


        private string GenerateJwtToken(Student student)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, student.Id.ToString()),
                new Claim(ClaimTypes.Email, student.Email),
                new Claim(ClaimTypes.Name, student.FirstName),
                new Claim(ClaimTypes.GivenName, $"{student.FirstName} {student.LastName}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Issuer"],
                audience: _configuration["Audience"],
                claims: claims,
                signingCredentials: creds,
                expires: DateTime.Now.AddHours(1));

            var newToken = new JwtSecurityTokenHandler().WriteToken(token);

            return newToken;
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
