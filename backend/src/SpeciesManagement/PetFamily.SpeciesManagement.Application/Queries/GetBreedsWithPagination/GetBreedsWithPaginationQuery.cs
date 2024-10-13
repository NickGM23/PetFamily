using PetFamily.Core.Abstractions;

namespace PetFamily.SpeciesManagement.Application.Queries.GetBreedsWithPagination
{
    public record GetBreedsWithPaginationQuery(Guid SpeciesId, int Page, int PageSize) : IQuery;
}
