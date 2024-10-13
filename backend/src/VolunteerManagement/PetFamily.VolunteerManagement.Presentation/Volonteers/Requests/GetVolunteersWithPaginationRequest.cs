using PetFamily.VolunteerManagement.Application.Queries.GetVolunteersWithPagination;

namespace PetFamily.VolunteerManagement.Presentation.Volonteers.Requests
{
    public record GetVolunteersWithPaginationRequest(int Page, int PageSize)
    {
        public GetVolunteersWithPaginationQuery ToQuery() =>
            new(Page, PageSize);
    }
}
