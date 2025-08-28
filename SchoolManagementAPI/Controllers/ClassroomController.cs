using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain;
using SchoolManagement.Domain.UserManagement;
using SchoolManagement.Dto;
using SchoolManagement.Persistence;

namespace SchoolManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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


    }
}
