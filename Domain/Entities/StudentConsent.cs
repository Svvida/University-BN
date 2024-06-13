using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    internal class StudentConsent
    {
        public Guid Id { get; set; }
        public bool PermissionForPhoto { get; set; }
        public bool PermissionForDataProcessing { get; set; }
        public Student? Student { get; set; }
    }
}
