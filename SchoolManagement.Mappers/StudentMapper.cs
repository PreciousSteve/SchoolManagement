
using SchoolManagement.Domain.UserManagement;
using SchoolManagement.Dto;

namespace SchoolManagement.Mappers
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
                PasswordHash = dto.Password, // hash later
                Gender = dto.Gender,
                UserStatus = UserStatus.Active
            };
        }
    }
}
