using SchoolManagement.Domain.UserManagement;
using SchoolManagement.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Mappers
{
    public static class TeacherMapper
    {
        public static Teacher ToEntity(this TeacherCreateDto dto)
        {
            return new Teacher
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Gender = dto.Gender,
                UserStatus = UserStatus.Active
            };
        }
    }
}
