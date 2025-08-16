

using SchoolManagement.Domain.UserManagement;

namespace SchoolManagement.Domain
{
    public class Enrollment
    {
        public int Id { get; set; }
        public Student Student { get; set; } = default!;
        public Course Course { get; set; } = default!;
    }

}
