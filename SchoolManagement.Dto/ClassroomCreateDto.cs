using SchoolManagement.Domain;
using SchoolManagement.Domain.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Dto
{
    public class ClassroomCreateDto
    {
        public required string Name { get; set; }
        public string Description { get; set; } = default!;
        public int TeacherId { get; set; }
    }
}
