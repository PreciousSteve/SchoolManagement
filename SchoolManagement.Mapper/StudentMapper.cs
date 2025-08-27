using SchoolManagement.Domain.UserManagement;
using SchoolManagement.Dto;

namespace SchoolManagement.Mapper
{
    public static class StudentMapper
    {
        public static Student ToEntity(this StudentCreateDto dto)
        {
            return new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                //PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                PasswordHash = dto.Password,
                Gender = dto.Gender,
                UserStatus = UserStatus.Active
            };
        }
    }
}
