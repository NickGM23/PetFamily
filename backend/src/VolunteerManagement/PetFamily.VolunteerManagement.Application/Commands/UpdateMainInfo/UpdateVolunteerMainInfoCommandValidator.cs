
using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdateMainInfo
{
    public class UpdateVolunteerMainInfoCommandValidator : AbstractValidator<UpdateVolunteerMainInfoCommand>
    {
        public UpdateVolunteerMainInfoCommandValidator()
        {
            RuleFor(v => v.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());

            RuleFor(v => v.UpdateVolunteerMainInfoDto.FullNameDto).MustBeValueObject(x => FullName.Create(x.LastName, x.FirstName));

            RuleFor(v => v.UpdateVolunteerMainInfoDto.Email).MustBeValueObject(Email.Create);

            RuleFor(v => v.UpdateVolunteerMainInfoDto.Description).MustBeValueObject(Description.Create);

            RuleFor(v => v.UpdateVolunteerMainInfoDto.YearsExperience).MustBeValueObject(YearsExperience.Create);

            RuleFor(v => v.UpdateVolunteerMainInfoDto.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
        }
    }
}
