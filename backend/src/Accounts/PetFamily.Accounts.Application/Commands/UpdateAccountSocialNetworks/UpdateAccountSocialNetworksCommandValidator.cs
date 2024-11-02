using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel.ValueObjects;


namespace PetFamily.Accounts.Application.Commands.UpdateAccountSocialNetworks
{
    public class UpdateAccountSocialNetworksCommandValidator : AbstractValidator<UpdateAccountSocialNetworksCommand>
    {
        public UpdateAccountSocialNetworksCommandValidator()
        {
            RuleFor(l => l.UserId).NotEmpty();

            RuleForEach(l => l.SocialNetworks)
                .MustBeValueObject(l => SocialNetwork.Create(l.Name, l.Link));
        }
    }
}
