using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdateMainInfo
{
    public record UpdateVolunteerMainInfoCommand(
        Guid VolunteerId, 
        UpdateVolunteerMainInfoDto UpdateVolunteerMainInfoDto) : ICommand;
}
