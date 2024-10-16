namespace PetFamily.Core.Dtos
{
    public record FullNameDto(string LastName,
                              string FirstName,
                              string? Patronymic = null);
}
