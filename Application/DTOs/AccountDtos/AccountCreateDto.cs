using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.AccountDtos
{
    public class AccountCreateDto : AccountOnlyDto
    {
        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
