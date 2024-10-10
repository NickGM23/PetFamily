
using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.SetMainPetPhoto
{
    public class SetMainPetPhotoValidator : AbstractValidator<SetMainPetPhotoCommand>
    {
        public SetMainPetPhotoValidator()
        {
            RuleFor(v => v.VolunteerId).NotEmpty()
                .WithError(Errors.General.ValueIsRequired("Volunteer id"));

            RuleFor(v => v.PetId).NotEmpty()
                .WithError(Errors.General.ValueIsRequired("Pet id"));

            RuleFor(u => u.FileName).NotEmpty()
                .WithError(Errors.General.ValueIsRequired("File name"));
        }
    }
}
