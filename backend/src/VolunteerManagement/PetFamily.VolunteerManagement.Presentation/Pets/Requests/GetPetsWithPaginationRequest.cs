using PetFamily.VolunteerManagement.Application.Queries.GetPetsWithPagination;
using PetFamily.VolunteerManagement.Domain.Enums;

namespace PetFamily.VolunteerManagement.Presentation.Pets.Requests
{
    public record GetPetsWithPaginationRequest(
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
        bool? IsVaccinated)
    {
        public GetPetsWithPaginationQuery ToQuery() =>
            new(
                Page,
                PageSize,
                SortBy,
                SortDirection,
                VolunteerId,
                Name,
                Description,
                Color,
                SpeciesId,
                BreedId,
                Country,
                City,
                Street,
                PostalCode,
                HouseNumber,
                FlatNumber,
                MinHeight,
                MaxHeight,
                MinWeight,
                MaxWeight,
                MinAge,
                MaxAge,
                Phone,
                HelpStatus,
                IsCastrated,
                IsVaccinated);
    }
}
