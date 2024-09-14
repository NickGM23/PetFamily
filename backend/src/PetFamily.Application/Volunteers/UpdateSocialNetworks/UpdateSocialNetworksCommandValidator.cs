using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateSocialNetworks
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
