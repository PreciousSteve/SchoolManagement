

namespace SchoolManagement.Domain
{
    public class Course
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int ClassRoomId { get; set; }
        public Classroom ClassRoom { get; set; } = default!;
        public List<Enrollment>? Enrollments { get; set; }
    }
}
