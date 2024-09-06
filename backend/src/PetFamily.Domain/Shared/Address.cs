using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared
{
    public record Address
    {
        public string Country { get; } = string.Empty;

        public string City { get; } = string.Empty;

        public string Street { get; } = string.Empty;

        public int PostalCode { get; }

        public string HouseNumber { get; } = string.Empty;

        public string? FlatNumber { get; }

        private Address(string country,
                        string city,
                        string street,
                        int postalCode,
                        string houseNumber,
                        string? flatNumber)
        {
            Country = country;
            City = city;
            Street = street;
            PostalCode = postalCode;
            HouseNumber = houseNumber;
            FlatNumber = flatNumber;
        }

        public static Result<Address, Error> Create(string country,
                                             string city,
                                             string street,
                                             int postalCode,
                                             string houseNumber,
                                             string? flatNumber)
        {
            if (string.IsNullOrEmpty(country))
            {
                return Errors.General.ValueIsInvalid("Country");
            }
            if (string.IsNullOrEmpty(city))
            {
                return Errors.General.ValueIsInvalid("City");
            }
            if (string.IsNullOrEmpty(street))
            {
                return Errors.General.ValueIsInvalid("Street");
            }
            if (postalCode <= 0)
            {
                return Errors.General.ValueIsInvalid("PostalCode");
            }
            if (string.IsNullOrEmpty(houseNumber))
            {
                return Errors.General.ValueIsInvalid("HouseNumber");
            }
            var address = new Address(country,
                                      city,
                                      street,
                                      postalCode,
                                      houseNumber,
                                      flatNumber);
            return address;
        }
    }
}
