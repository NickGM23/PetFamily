using PetFamily.Core.Dtos;
using PetFamily.VolunteerManagement.Application.Commands.UpdateRequisites;

namespace PetFamily.VolunteerManagement.Presentation.Volonteers.Requests
{
    public record UpdateRequisitesRequest(IEnumerable<RequisiteDto> requisites)
    {
        public UpdateRequisitesCommand ToCommand(Guid volunteerId) =>
            new(volunteerId, requisites);
    }
}
