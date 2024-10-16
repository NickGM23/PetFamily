using PetFamily.Core.Abstractions;

namespace PetFamily.SpeciesManagement.Application.Queries.GetSpeciesWithPanination
{
    public record GetSpeciesWithPaginationQuery(int Page, int PageSize) : IQuery;
}
