using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Commands.SetMainPetPhoto
{
    public record SetMainPetPhotoCommand(Guid VolunteerId, Guid PetId, string FileName) : ICommand;
}
