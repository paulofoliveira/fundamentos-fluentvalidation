﻿namespace DomainModel
{
    public class Address
    {
        public Address(string street, string city, string state, string zipCode)
        {
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }
    }
}
