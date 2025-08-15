

using SchoolManagement.Domain.UserManagement;

namespace SchoolManagement.Domain
{
    public class Enrollment
    {
        public int Id { get; set; }
        public required Student StudentId { get; set; }
        public required Course CourseId { get; set; }
    }
}
