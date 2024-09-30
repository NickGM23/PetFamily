using PetFamily.Application.Species.Queries.GetSpeciesWithPanination;

namespace PetFamily.API.Controllers.Species.Requests
{
    public record GetSpeciesWithPaginationRequest(int Page, int PageSize)
    {
        public GetSpeciesWithPaginationQuery ToQuery() =>
            new(Page, PageSize);
    }
}
