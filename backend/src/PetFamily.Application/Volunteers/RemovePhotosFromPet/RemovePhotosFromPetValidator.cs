using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.RemovePhotosFromPet
{
    public class RemovePhotosFromPetValidator : AbstractValidator<RemovePhotosFromPetCommand>
    {
        public RemovePhotosFromPetValidator()
        {
            RuleFor(v => v.VolunteerId).NotEmpty()
                .WithError(Errors.General.ValueIsRequired("Volunteer id"));

            RuleFor(v => v.PetId).NotEmpty()
                .WithError(Errors.General.ValueIsRequired("Pet id"));

            RuleFor(u => u.BucketName).NotEmpty()
                .WithError(Errors.General.ValueIsRequired("Bucket name"));
        }
    }
}
