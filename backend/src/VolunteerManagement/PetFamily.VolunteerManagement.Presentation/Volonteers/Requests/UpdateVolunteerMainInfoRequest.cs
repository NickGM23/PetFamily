using PetFamily.Core.Dtos;
using PetFamily.VolunteerManagement.Application.Commands.UpdateMainInfo;

namespace PetFamily.VolunteerManagement.Presentation.Volonteers.Requests
{
    public record UpdateVolunteerMainInfoRequest(UpdateVolunteerMainInfoDto updateVolunteerMainInfoDto)
    {
        public UpdateVolunteerMainInfoCommand ToCommand(Guid volunteerId) =>
            new(volunteerId, updateVolunteerMainInfoDto);
    }
}
