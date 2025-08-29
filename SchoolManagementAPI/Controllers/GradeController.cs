using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain;
using SchoolManagement.Dto;
using SchoolManagement.Persistence;
using System.Security.Claims;

namespace SchoolManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GradeController : ControllerBase
    {
        private readonly SchoolManagementDbContext _context;
        public GradeController(SchoolManagementDbContext context)
        {
            _context = context;
        }

        [HttpPost("assign")]
        public async Task<ActionResult> AssignGrade(GradeCreateDto dto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = identity.FindFirst(ClaimTypes.Role).Value;

            if (role != "Teacher")
            {
                return Unauthorized("You cannot Grade this Enrollment");
            }
            var enrollment = await _context.Enrollments.FindAsync(dto.EnrollmentId);
            if (enrollment == null)
            {
                return NotFound(new { message = "Enrollment not found" });
            }
            var grade = new Grade
            {
                Score = dto.Score,
                EnrollmentId = dto.EnrollmentId,
            };
            await _context.Grades.AddAsync(grade);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Grade Assigned Successfully." });
        }

        [HttpGet]
        public async Task<ActionResult<List<Grade>>> GetAllGrades()
        {
            return await _context.Grades.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Grade>> GetGrade(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound($"Grade with ID {id} not found.");
            }
            return Ok(grade);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Grade>> UpdateGrade(int id, GradeCreateDto dto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = identity.FindFirst(ClaimTypes.Role).Value;
            if (role != "Teacher")
            {
                return Unauthorized("You cannot Update this Grade");
            }
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound(new { message = $"Grade with ID {id} does not exist" });
            }
            grade.Score = dto.Score;
            grade.EnrollmentId = dto.EnrollmentId;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new { message = $"Grade with ID {id} does not exist" });
            }
            return Ok(grade);
        }
    }
}
