using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Application.Commands.RemovePhotosFromPet
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
