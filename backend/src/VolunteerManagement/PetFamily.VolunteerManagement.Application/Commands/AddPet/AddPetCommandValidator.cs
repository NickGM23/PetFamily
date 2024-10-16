
using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.VolunteerManagement.Application.Commands.AddPet
{
    public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
    {
        public AddPetCommandValidator()
        {
            RuleFor(a => a.VolunteerId).NotEmpty()
                .WithError(Errors.General.ValueIsRequired("Volunteer id"));
            RuleFor(a => a.Name).MustBeValueObject(Name.Create);
            RuleFor(a => a.Description).MustBeValueObject(Description.Create);
            RuleFor(a => a.Color).MustBeValueObject(a => LowTextLength.Create(a, "Color"));
            RuleFor(a => a.HealthInfo).MustBeValueObject(a => HighTextLength.Create(a, "Health info"));

            RuleFor(a => a.Address)
                .MustBeValueObject(a => Address.Create(
                    a.Country,
                    a.City,
                    a.Street,
                    a.PostalCode,
                    a.HouseNumber,
                    a.FlatNumber));

            RuleFor(a => a.Weight);
            RuleFor(a => a.Height);
            RuleFor(a => a.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
            RuleFor(a => a.IsCastrated);
            RuleFor(a => a.BirthDate).NotEmpty().WithError(Errors.General.ValueIsRequired());
            RuleFor(a => a.IsVaccinated);
            RuleFor(a => a.HelpStatus).NotNull().WithError(Errors.General.ValueIsInvalid());

            RuleForEach(c => c.Requisites)
                .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
        }
    }
}
