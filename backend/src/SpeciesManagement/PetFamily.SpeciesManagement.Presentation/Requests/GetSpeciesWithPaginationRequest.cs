using PetFamily.SpeciesManagement.Application.Queries.GetSpeciesWithPanination;

namespace PetFamily.SpeciesManagement.Presentation.Requests
{
    public record GetSpeciesWithPaginationRequest(int Page, int PageSize)
    {
        public GetSpeciesWithPaginationQuery ToQuery() =>
            new(Page, PageSize);
    }
}
