using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain
{
    public class Grade
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public Enrollment EnrollmentId { get; set; }
    }
}
