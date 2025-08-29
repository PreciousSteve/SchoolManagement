using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain.UserManagement;
using SchoolManagement.Dto;
using SchoolManagement.Persistence;
using System.Security.Claims;

namespace SchoolManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeacherController : ControllerBase
    {
        private readonly SchoolManagementDbContext _context;
        public TeacherController(SchoolManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Teacher>>> GetAllTeachers()
        {
            return await _context.Teachers.Include(t => t.ClassRoom).ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacherById(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound("Teacher is not Found");
            }
            return teacher;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Teacher>> UpdateTeacher(int id, TeacherCreateDto dto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var role = identity.FindFirst(ClaimTypes.Role).Value;
            if (role != "Teacher")
            {
                return Unauthorized("You cannot Update this Teacher");
            }
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound(new { message = $"Teacher with ID {id} does not exist" });
            }

            var emailExists = await _context.Teachers.AnyAsync(s => s.Email == dto.Email && s.Id != id);
            if (emailExists)
            {
                return BadRequest(new { message = "Email is already in use by another teacher" });
            }

            teacher.FirstName = dto.FirstName;
            teacher.LastName = dto.LastName;
            teacher.Email = dto.Email;
            teacher.PasswordHash = dto.Password;
            teacher.Gender = dto.Gender;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "A concurrency error occurred while updating the teacher." });
            }

            return Ok(teacher);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeacher(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var role = identity.FindFirst(ClaimTypes.Role).Value;
            if (role != "Teacher")
            {
                return Unauthorized("You cannot Delete this Teacher");
            }
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound(new { message = $"Teacher with ID {id} does not exist" });
            }
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return NoContent();

        }
    }
}
