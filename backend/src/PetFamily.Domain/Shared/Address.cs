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

        public static Result<Address> Create(string country,
                                             string city,
                                             string street,
                                             int postalCode,
                                             string houseNumber,
                                             string? flatNumber)
        {
            if (string.IsNullOrEmpty(country))
            {
                return Result.Failure<Address>("Country can not be empty");
            }
            if (string.IsNullOrEmpty(city))
            {
                return Result.Failure<Address>("City can not be empty");
            }
            if (string.IsNullOrEmpty(street))
            {
                return Result.Failure<Address>("Street can not be empty");
            }
            if (postalCode <= 0)
            {
                return Result.Failure<Address>("Incorrect value for field PostalCode");
            }
            if (string.IsNullOrEmpty(houseNumber))
            {
                return Result.Failure<Address>("HouseNumber can not be empty");
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
