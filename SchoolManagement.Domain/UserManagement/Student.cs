namespace SchoolManagement.Domain.UserManagement
{
    public class Student
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string PasswordHash { get; set; } = default!;
        public required string Email { get; set; }
        public Gender Gender { get; set; }
        public UserStatus UserStatus { get; private set; }
        public Classroom ClassRoom { get; set; }
        public List<Enrollment> Enrollments { get; set; }
    }
}
