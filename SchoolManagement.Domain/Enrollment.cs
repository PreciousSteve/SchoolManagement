using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain
{
    public class Enrollment
    {
        public required Student StudentId { get; set; }
        public required Course CourseId { get; set; }
    }
}
