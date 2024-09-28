
using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Species.CreateSpecies
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
