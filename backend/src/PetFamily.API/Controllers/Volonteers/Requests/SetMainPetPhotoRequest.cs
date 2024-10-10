using PetFamily.Application.Volunteers.SetMainPetPhoto;

namespace PetFamily.API.Controllers.Volonteers.Requests
{
    public record SetMainPetPhotoRequest(Guid PetId, string FileName)
    {
        public SetMainPetPhotoCommand ToCommand(Guid VolunteerId) =>
            new(VolunteerId, PetId, FileName);
    }
}
