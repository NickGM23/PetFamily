
using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.SpeciesManagement.Application.Commands.AddBreed
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
