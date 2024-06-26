﻿using Domain.EntitiesBase;

namespace Domain.Entities.StudentEntities
{
    public class StudentAddress : AddressBase
    {
        public Student? Student { get; set; }

        public StudentAddress() { }

        public StudentAddress(Guid id, string country, string city, string postalCode, string street, string buildingNumber, string? apartmentNumber)
            : base(id, country, city, postalCode, street, buildingNumber, apartmentNumber) { }
    }
}
