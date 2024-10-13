using PetFamily.SpeciesManagement.Application.Queries.GetBreedsWithPagination;

namespace PetFamily.SpeciesManagement.Presentation.Requests
{
    public record GetBreedsWithPaginationRequest(int Page, int PageSize)
    {
        public GetBreedsWithPaginationQuery ToQuery(Guid speciesId) =>
            new(speciesId, Page, PageSize);
    }
}
