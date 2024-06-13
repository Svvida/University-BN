using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    internal class EmployeeAddress
    {
        public Guid Id { get; set; }
        public string Country { get; set; } = "Poland";
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = "00-000";
        public string Street { get; set; } = string.Empty;
        public string BuildingNumber { get; set; } = string.Empty;
        public string? ApartmentNumber { get; set; }
        public Employee? Employee { get; set; }
    }
}
