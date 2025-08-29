using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain;
using SchoolManagement.Domain.UserManagement;
using SchoolManagement.Dto;
using SchoolManagement.Persistence;
using System.Security.Claims;

namespace SchoolManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClassroomController : ControllerBase
    {
        private readonly SchoolManagementDbContext _context;
        public ClassroomController(SchoolManagementDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Classroom>> CreateClassroom(ClassroomCreateDto dto)
        {
            var classroomExists = await _context.Classrooms.AnyAsync(c => c.Name == dto.Name);

            if (classroomExists)
            {
                return Conflict(new { message = "Classroom name already exists" });
            }
            var teacherExists = await _context.Teachers.FindAsync(dto.TeacherId);
            if (teacherExists == null)
            {
                return NotFound(new { message = "Teacher not found" });
            }
            var classroom = new Classroom
            {
                Name = dto.Name,
                Description = dto.Description,
                TeacherId = dto.TeacherId
            };

            await _context.Classrooms.AddAsync(classroom);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Classroom Created Successfully." });
        }


        [HttpGet]
        public async Task<ActionResult<List<ClassroomReadDto>>> GetAllClassrooms()
        {
            var classrooms = await _context.Classrooms
                .Include(c => c.Teacher)
                .Include(c => c.Courses)
                .Select(c => new ClassroomReadDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    TeacherName = c.Teacher.FirstName + " " + c.Teacher.LastName,
                    Courses = c.Courses!.Select(course => course.Title).ToList()
                })
                .ToListAsync();

            return Ok(classrooms);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Classroom>> GetClassroom(int id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);

            if (classroom == null)
            {
                return NotFound($"Classroom with ID {id} not found.");
            }

            return Ok(classroom);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Classroom>> UpdateClassroom(int id, ClassroomCreateDto dto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var role = identity.FindFirst(ClaimTypes.Role).Value;

            if (role != "Teacher")
            {
                return Unauthorized("You cannot Update this Classroom");
            }

            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null)
            {
                return NotFound(new { message = $"Classroom with ID {id} does not exist" });
            }

            classroom.Name = dto.Name;
            classroom.Description = dto.Description;
            classroom.TeacherId = dto.TeacherId;    

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "A concurrency error occurred while updating the teacher." });
            }

            return Ok(classroom);
        }


    }
}
