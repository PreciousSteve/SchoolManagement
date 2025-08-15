

namespace SchoolManagement.Domain
{
    public class Enrollment
    {
        public required Student StudentId { get; set; }
        public required Course CourseId { get; set; }
    }
}
