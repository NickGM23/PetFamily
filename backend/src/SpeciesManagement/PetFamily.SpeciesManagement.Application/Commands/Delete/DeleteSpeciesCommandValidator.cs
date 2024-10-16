
using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.SpeciesManagement.Application.Commands.Delete
{
    public class DeleteSpeciesCommandValidator : AbstractValidator<DeleteSpeciesCommand>
    {
        public DeleteSpeciesCommandValidator()
        {
            RuleFor(v => v.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        }
    }
}
