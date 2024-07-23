using Application.DTOs.Account.Dtos;
using Application.DTOs.BaseDtos;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Student.Dtos
{
    public class StudentOnlyDto : PersonOnlyDto
    {

    }

    public class StudentFullDto : StudentOnlyDto
    {
        public AccountOnlyDto Account { get; set; }
        public StudentAddressDto Address { get; set; }
        public StudentConsentDto Consent { get; set; }
    }


    public class StudentCreateDto : PersonOnlyDto
    {
        [Required]
        public AccountOnlyDto Account { get; set; }

        [Required]
        public StudentAddressDto Address { get; set; }

        [Required]
        public StudentConsentDto Consent { get; set; }
    }
}
