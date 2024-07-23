using Application.DTOs.Account.Dtos;
using Application.DTOs.BaseDtos;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Employee.Dtos
{
    public class EmployeeOnlyDto : PersonOnlyDto
    {
    }

    public class EmployeeFullDto : PersonOnlyDto
    {
        public AccountOnlyDto Account { get; set; }
        public EmployeeAddressDto Address { get; set; }
        public EmployeeConsentDto Consent { get; set; }
    }

    public class EmployeeCreateDto : PersonOnlyDto
    {
        [Required]
        public AccountOnlyDto Account { get; set; }

        [Required]
        public EmployeeAddressDto Address { get; set; }

        [Required]
        public EmployeeConsentDto Consent { get; set; }
    }
}
