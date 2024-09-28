
using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Species.AddBreed
{
    public class AddBreedCommandValidator : AbstractValidator<AddBreedCommand>
    {
        public AddBreedCommandValidator()
        {
            RuleFor(a => a.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired());
            RuleFor(a => a.Name).MustBeValueObject(Name.Create);
            RuleFor(a => a.Description).MustBeValueObject(Description.Create);
        }
    }
}
