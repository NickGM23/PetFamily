
using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.Queries.GetSpeciesWithPanination
{
    public record GetSpeciesWithPaginationQuery(int Page, int PageSize) : IQuery;
}
