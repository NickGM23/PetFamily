using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.RemoveFilesFromPet
{
    public class RemoveFilesFromPetValidator : AbstractValidator<RemoveFilesFromPetCommand>
    {
        public RemoveFilesFromPetValidator()
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
