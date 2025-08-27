using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Dto;
using SchoolManagement.Persistence;
using SchoolManagement.Mappers;
using SchoolManagement.Domain.UserManagement;


namespace SchoolManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly SchoolManagementDbContext _context;
        public StudentController(SchoolManagementDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult> CreateStudent([FromBody] StudentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingStudent = await _context.Students.AnyAsync(s => s.Email == dto.Email);
            if (existingStudent)
            {
                return BadRequest(new { message = "This Email already exists in the system."});
            }

            var student = dto.ToEntity();

            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Student created successfully"});
        }

        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            return await _context.Students.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound("Student is not Found");
            }
            return student;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> UpdateStudent(int id, StudentCreateDto dto)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = $"Student with ID {id} does not exist" });
            }

            var emailExists = await _context.Students.AnyAsync(s => s.Email == dto.Email && s.Id != id);
            if (emailExists)
            {
                return BadRequest(new { message = "Email is already in use by another student" });
            }

            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.Email = dto.Email;
            student.PasswordHash = dto.Password;
            student.Gender = dto.Gender;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "A concurrency error occurred while updating the student." });
            }

            return Ok(student);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = $"Student with ID {id} does not exist" });
            }
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();

        }


    }
}
