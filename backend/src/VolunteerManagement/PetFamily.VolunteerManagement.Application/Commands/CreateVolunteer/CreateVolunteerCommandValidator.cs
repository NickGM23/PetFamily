using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.VolunteerManagement.Application.Commands.CreateVolunteer
{
    public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
    {
        public CreateVolunteerCommandValidator()
        {
            RuleFor(v => v.FullName).MustBeValueObject(x => FullName.Create(x.LastName, x.FirstName));

            RuleFor(v => v.Email).MustBeValueObject(Email.Create);

            RuleFor(v => v.Description).MustBeValueObject(Description.Create);

            RuleFor(v => v.YearsExperience).MustBeValueObject(YearsExperience.Create);

            RuleFor(v => v.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

            RuleForEach(v => v.SocialNetworks)
                .MustBeValueObject(x =>
                    SocialNetwork.Create(
                        x.Link,
                        x.Name));

            RuleForEach(v => v.Requisites)
                .MustBeValueObject(x =>
                    Requisite.Create(
                        x.Name,
                        x.Description));
        }
    }
}
