
using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Volunteers.SetMainPetPhoto
{
    public record SetMainPetPhotoCommand(Guid VolunteerId, Guid PetId, string FileName) : ICommand;
}
