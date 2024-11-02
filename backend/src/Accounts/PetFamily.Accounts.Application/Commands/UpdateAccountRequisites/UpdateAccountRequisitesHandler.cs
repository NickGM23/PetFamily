using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Extensions;


namespace PetFamily.Accounts.Application.Commands.UpdateAccountRequisites
{
    public class UpdateAccountRequisitesHandler(
    IValidator<UpdateAccountRequisitesCommand> validator,
    UserManager<User> userManager,
    IVolunteerAccountManager volunteerAccountManager,
    CancellationToken cancellationToken = default) : ICommandHandler<UpdateAccountRequisitesCommand>
    {
        public async Task<UnitResult<ErrorList>> Handle(
            UpdateAccountRequisitesCommand command, CancellationToken token = default)
        {
            var validationResult = await validator.ValidateAsync(command, token);
            if (!validationResult.IsValid)
                return validationResult.ToList();

            var user = await userManager.FindByIdAsync(command.UserId.ToString());
            if (user is null)
                return Errors.User.InvalidCredentials().ToErrorList();

            var requisites = command.Requisites.Select(s => Requisite.Create(s.Name, s.Description).Value);

            var volunteerAccount = await volunteerAccountManager.GetVolunteerAccountByIdAsync(user.Id, token);
            if (volunteerAccount is null)
                return Errors.General.NotFound(command.UserId).ToErrorList();

            volunteerAccount.Requisites = RequisiteList.Create(requisites.ToList()).Value;
            await volunteerAccountManager.UpdateAsync(volunteerAccount, cancellationToken);

            return UnitResult.Success<ErrorList>();
        }
    }
}
