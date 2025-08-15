

namespace SchoolManagement.Domain
{
    public class Classroom
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = default!;
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public List<Student> Students { get; set; }
        public List<Course> Courses { get; set; }
    }
}
