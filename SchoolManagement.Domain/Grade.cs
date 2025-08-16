

namespace SchoolManagement.Domain
{
    public class Grade
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = default!;
    }
}
