

namespace SchoolManagement.Domain
{
    public class Grade
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public Enrollment EnrollmentId { get; set; }
    }
}
