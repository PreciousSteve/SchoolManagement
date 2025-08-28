using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Domain.UserManagement
{
    public class Teacher
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public string PasswordHash { get; set; } = default!;
        public required string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format.")]
        public required string Email { get; set; }
        public string Phone { get; set; } = default!;
        public Gender Gender { get; set; }
        public UserStatus UserStatus { get; set; }
        public Classroom ClassRoom { get; set; } = default!;
    }
}
