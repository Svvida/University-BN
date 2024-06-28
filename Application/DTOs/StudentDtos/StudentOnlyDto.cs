using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.StudentDtos
{
    public abstract class StudentOnlyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; } = 0;
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public DateTime DateOfAddmission { get; set; }
    }
}
