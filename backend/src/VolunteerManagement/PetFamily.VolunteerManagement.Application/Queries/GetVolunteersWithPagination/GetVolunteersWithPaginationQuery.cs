using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Queries.GetVolunteersWithPagination
{
    public record GetVolunteersWithPaginationQuery(int Page, int PageSize) : IQuery;
}
