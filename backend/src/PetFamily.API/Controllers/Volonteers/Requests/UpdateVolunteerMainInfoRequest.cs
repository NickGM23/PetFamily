
using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.UpdateMainInfo
{
    public record UpdateVolunteerMainInfoRequest(UpdateVolunteerMainInfoDto updateVolunteerMainInfoDto)
    {
        public UpdateVolunteerMainInfoCommand ToCommand(Guid volunteerId) =>
            new(volunteerId, updateVolunteerMainInfoDto);
    }
}
