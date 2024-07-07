using System.ComponentModel.DataAnnotations;

namespace Domain.EntitiesBase
{
    public abstract class AddressBase
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Country { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string City { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Street { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 1)]
        public string BuildingNumber { get; set; }

        [StringLength(10, MinimumLength = 1)]
        public string? ApartmentNumber { get; set; }

        protected AddressBase()
        {
        }

        protected AddressBase(Guid id, string country, string city, string postalCode, string street, string buildingNumber, string? apartmentNumber)
        {
            Id = id;
            Country = country;
            City = city;
            PostalCode = postalCode;
            Street = street;
            BuildingNumber = buildingNumber;
            ApartmentNumber = apartmentNumber;
        }
    }
}
