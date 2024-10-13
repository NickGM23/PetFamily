using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdateRequisites
{
    public record UpdateRequisitesCommand(
        Guid VolunteerId, 
        IEnumerable<RequisiteDto> Requisites) : ICommand;
}
