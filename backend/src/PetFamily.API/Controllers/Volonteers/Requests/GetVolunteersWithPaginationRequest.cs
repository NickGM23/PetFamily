using PetFamily.Application.Volunteers.Queries.GetVolunteersWithPagination;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record GetVolunteersWithPaginationRequest(int Page, int PageSize)
    {
        public GetVolunteersWithPaginationQuery ToQuery() =>
            new(Page, PageSize);
    }
}
