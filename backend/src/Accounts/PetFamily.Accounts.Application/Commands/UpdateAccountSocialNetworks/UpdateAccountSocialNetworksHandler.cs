using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel;
using PetFamily.SharedKernel.ValueObjects;


namespace PetFamily.Accounts.Application.Commands.UpdateAccountSocialNetworks
{
    public class UpdateAccountSocialNetworksHandler(
    IValidator<UpdateAccountSocialNetworksCommand> validator,
    UserManager<User> userManager) : ICommandHandler<UpdateAccountSocialNetworksCommand>
{
    public async Task<UnitResult<ErrorList>> Handle(
        UpdateAccountSocialNetworksCommand command, CancellationToken token = default)
    {
        var validationResult = await validator.ValidateAsync(command, token);
        if (!validationResult.IsValid)
            return validationResult.ToList();

        var user = await userManager.FindByIdAsync(command.UserId.ToString());
        if (user is null)
            return Errors.User.InvalidCredentials().ToErrorList();

        var socialLinks = command.SocialNetworks.Select(s => SocialNetwork.Create(s.Link, s.Name).Value);
        user.SocialNetworks = new SocialNetworkList(socialLinks.ToList());

        await userManager.UpdateAsync(user);

        return UnitResult.Success<ErrorList>();
    }
}
}
