
using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.Delete
{
    public class DeleteSpeciesCommandValidator : AbstractValidator<DeleteSpeciesCommand>
    {
        public DeleteSpeciesCommandValidator()
        {
            RuleFor(v => v.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        }
    }
}
