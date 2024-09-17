
using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateMainInfo
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
