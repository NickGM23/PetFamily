using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;
using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdateSocialNetworks
{
    public class UpdateSocialNetworksCommandValidator : AbstractValidator<UpdateSocialNetworksCommand>
    {
        public UpdateSocialNetworksCommandValidator()
        {
            RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
            RuleForEach(u => u.SocialNetworks)
                .MustBeValueObject(r => SocialNetwork.Create(r.Name, r.Link));
        }
    }
}
