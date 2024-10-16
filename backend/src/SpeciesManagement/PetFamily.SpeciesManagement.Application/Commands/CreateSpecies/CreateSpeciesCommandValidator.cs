
using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.SpeciesManagement.Application.Commands.CreateSpecies
{
    public class CreateSpeciesCommandValidator : AbstractValidator<CreateSpeciesCommand>
    {
        public CreateSpeciesCommandValidator()
        {
            RuleFor(v => v.Name).MustBeValueObject(Name.Create);

            RuleFor(v => v.Description).MustBeValueObject(Description.Create);
        }
    }
}
