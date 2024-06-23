using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.EntitiesBase
{
    public abstract class AddressBase
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Country { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(10)]
        public string PostalCode { get; set; }

        [Required]
        [MaxLength(50)]
        public string Street { get; set; }

        [Required]
        [MaxLength(10)]
        public string BuildingNumber { get; set; }

        [MaxLength(10)]
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
