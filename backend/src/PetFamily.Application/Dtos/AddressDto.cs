
namespace PetFamily.Application.Dtos
{
    public record AddressDto(
        string Country, 
        string City, 
        string Street,
        int PostalCode,
        string HouseNumber,
        string FlatNumber);
}
