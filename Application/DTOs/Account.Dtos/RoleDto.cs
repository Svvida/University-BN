﻿using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.AccountDtos
{
    public class RoleDto
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(50)]
        public string NormalizedName { get; set; }
    }
}
