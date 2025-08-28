using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Domain.UserManagement
{
    public class Student
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string PasswordHash { get; set; } = default!;
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format.")]
        public required string Email { get; set; }
        public Gender Gender { get; set; }
        public UserStatus UserStatus { get; set; }
        public List<Enrollment>? Enrollments { get; set; }
    }
}
