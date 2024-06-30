using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.BaseDtos
{
    public abstract class AddressOnlyDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Country { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string City { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Street { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 1)]
        public string BuildingNumber { get; set; }

        [StringLength(5, MinimumLength = 1)]
        public string? ApartmentNumber { get; set; }
    }
}
