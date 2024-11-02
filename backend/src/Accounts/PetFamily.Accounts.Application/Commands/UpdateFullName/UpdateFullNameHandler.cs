using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PetFamily.Core.Abstractions;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetFamily.Accounts.Domain;
using PetFamily.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace PetFamily.Accounts.Application.Commands.UpdateFullName
{
    public class UpdateFullNameHandler(
    UserManager<User> userManager,
    IValidator<UpdateFullNameCommand> validator) : ICommandHandler<UpdateFullNameCommand>
    {
        public async Task<UnitResult<ErrorList>> Handle(
            UpdateFullNameCommand command, CancellationToken cancellationToken = default)
        {
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (validationResult.IsValid == false)
                return validationResult.ToList();

            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);
            if (user is null)
                return Errors.General.NotFound(command.UserId).ToErrorList();

            var fullName = FullName.Create(command.FullNameDto.LastName,
                command.FullNameDto.FirstName,
                command.FullNameDto.Patronymic).Value;

            user.FullName = fullName;
            await userManager.UpdateAsync(user);

            return UnitResult.Success<ErrorList>();
        }
    }
}
