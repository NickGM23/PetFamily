
using PetFamily.Application.Abstractions;
using PetFamily.Domain.Enums;

namespace PetFamily.Application.Volunteers.Queries.GetPetsWithPagination
{
    public record GetPetsWithPaginationQuery(
        int Page,
        int PageSize,
        string? SortBy,
        string? SortDirection,
        Guid? VolunteerId,
        string? Name,
        string? Description,
        string? Color,
        Guid? SpeciesId,
        Guid? BreedId,
        string? Country,
        string? City,
        string? Street,
        int? PostalCode,
        string? HouseNumber,
        string? FlatNumber,
        int? MinHeight,
        int? MaxHeight,
        int? MinWeight,
        int? MaxWeight,
        int? MinAge,
        int? MaxAge,
        string? Phone,
        HelpStatus? HelpStatus,
        bool? IsCastrated,
        bool? IsVaccinated) : IQuery;
}
