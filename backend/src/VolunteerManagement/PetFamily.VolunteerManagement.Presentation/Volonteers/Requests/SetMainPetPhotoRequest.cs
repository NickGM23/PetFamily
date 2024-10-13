using PetFamily.VolunteerManagement.Application.Commands.SetMainPetPhoto;

namespace PetFamily.VolunteerManagement.Presentation.Volonteers.Requests
{
    public record SetMainPetPhotoRequest(Guid PetId, string FileName)
    {
        public SetMainPetPhotoCommand ToCommand(Guid VolunteerId) =>
            new(VolunteerId, PetId, FileName);
    }
}
