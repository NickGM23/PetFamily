using PetFamily.Application.Dtos;
using PetFamily.Application.Volunteers.UpdateRequisites;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record UpdateRequisitesRequest(IEnumerable<RequisiteDto> requisites)
    {
        public UpdateRequisitesCommand ToCommand(Guid volunteerId) =>
            new(volunteerId, requisites);
    }
}
