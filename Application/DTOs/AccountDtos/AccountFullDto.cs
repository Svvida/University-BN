using Application.DTOs.BaseDtos;
using Application.DTOs.EmployeeDtos;
using Application.DTOs.StudentDtos;
using System.Collections.Generic;

namespace Application.DTOs.AccountDtos
{
    public class AccountFullDto : AccountOnlyDto
    {
        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();
        public StudentOnlyDto Student { get; set; }
        public EmployeeOnlyDto Employee { get; set; }
    }
}
