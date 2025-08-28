using SchoolManagement.Domain.UserManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Dto
{
    public class TeacherCreateDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format.")]
        public required string Email { get; set; }
        public string Phone { get; set; } = default!;
        public required string Password { get; set; }
        public Gender Gender { get; set; }
    }
}
