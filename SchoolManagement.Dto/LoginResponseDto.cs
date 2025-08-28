using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Dto
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }
    }
}
