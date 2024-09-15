namespace PetFamily.Application.Dtos
{
    public record FullNameDto(string LastName,
                              string FirstName,
                              string? Patronymic = null);
}
