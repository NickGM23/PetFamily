using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.CreateVolunteer
{
    public class CreateVolunteerValidator : AbstractValidator<CreateVolunteerRequest>
    {
        public CreateVolunteerValidator()
        {
            RuleFor(v => v.FullName).MustBeValueObject(x => FullName.Create(x.LastName, x.FirstName));
            
            RuleFor(v => v.Email).MustBeValueObject(Email.Create);
            
            RuleFor(v => v.Description).MustBeValueObject(Description.Create);

            RuleFor(v => v.YearsOfExperience).NotNull().InclusiveBetween(0, 50).WithError(Errors.General.ValueIsInvalid("{PropertyValue}", "YearsOfExperience"));

            RuleFor(v => v.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

            RuleForEach(v => v.SocialNetworksDTO)
                .MustBeValueObject(x =>
                    SocialNetwork.Create(
                        x.Link,
                        x.Name));

            RuleForEach(v => v.RequisitesDTO)
                .MustBeValueObject(x => 
                    Requisite.Create(
                        x.Name,
                        x.Description));
        }
    }
}
