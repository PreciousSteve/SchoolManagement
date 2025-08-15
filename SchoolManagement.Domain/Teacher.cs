using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain
{
    public class Teacher
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public string PasswordHash { get; protected set; } = default!;
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string Phone { get; set; } = default!;
        public Gender Gender { get; set; }
        public UserStatus UserStatus { get; set; }
    }
}
