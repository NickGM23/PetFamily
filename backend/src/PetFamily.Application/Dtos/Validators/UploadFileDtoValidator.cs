
using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Dtos.Validators
{
    public class UploadFileDtoValidator : AbstractValidator<UploadFileDto>
    {
        public UploadFileDtoValidator()
        {
            RuleFor(u => u.FileName).NotEmpty().WithError(Errors.General.ValueIsRequired());
            RuleFor(u => u.Content).Must(c => c.Length < 5000000);
        }
    }
}