using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Queries.GetVolunteer
{
    public record GetVolunteerQuery(Guid Id) : IQuery;
}
