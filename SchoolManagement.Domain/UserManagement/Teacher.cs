namespace SchoolManagement.Domain.UserManagement
{
    public class Teacher
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public string PasswordHash { get; protected set; } = default!;
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string Phone { get; set; } = default!;
        public Gender Gender { get; set; }
        public UserStatus UserStatus { get; set; }
        public Classroom ClassRoom { get; set; } = default!;
    }
}
