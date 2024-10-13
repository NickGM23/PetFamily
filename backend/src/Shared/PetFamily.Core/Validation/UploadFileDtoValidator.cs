
using FluentValidation;
using PetFamily.Core.Dtos;
using PetFamily.SharedKernel;

namespace PetFamily.Core.Validation
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