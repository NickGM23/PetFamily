using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdatePet
{
    public class UpdatePetValidator : AbstractValidator<UpdatePetCommand>
    {
        public UpdatePetValidator()
        {
            RuleFor(v => v.VolunteerId).NotEmpty()
                .WithError(Errors.General.ValueIsRequired("Volunteer id"));

            RuleFor(v => v.PetId).NotEmpty()
                .WithError(Errors.General.ValueIsRequired("Pet id"));

            RuleFor(a => a.Name).MustBeValueObject(Name.Create);

            RuleFor(a => a.Description).MustBeValueObject(Description.Create);

            RuleFor(a => a.Color).MustBeValueObject(a => LowTextLength.Create(a, "Color"));

            RuleFor(c => c.HealthInfo).NotEmpty()
                .WithError(Errors.General.ValueIsRequired("Health info"));

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
