using PetFamily.Application.Dtos;

namespace PetFamily.Application.Volunteers.UpdateMainInfo
{
    public record UpdateVolunteerMainInfoCommand(Guid VolunteerId, UpdateVolunteerMainInfoDto UpdateVolunteerMainInfoDto);
}
