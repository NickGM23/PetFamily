
using FluentValidation;
using PetFamily.Application.Dtos.Validators;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UploadFilesToPet
{
    public class UploadFilesToPetCommandValidator : AbstractValidator<UploadFilesToPetCommand>
    {
        public UploadFilesToPetCommandValidator()
        {
            RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
            RuleFor(u => u.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
            RuleForEach(u => u.Files).SetValidator(new UploadFileDtoValidator());
        }
    }
}