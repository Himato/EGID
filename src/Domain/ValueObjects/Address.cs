﻿namespace EGID.Domain.ValueObjects
{
    public class Address
    {
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string PostalCode { get; set; }
        public string Country { get; }

        public Address() { }

        public Address(string street, string city, string state, string country, string postalCode)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            PostalCode = postalCode;
        }
    }
}