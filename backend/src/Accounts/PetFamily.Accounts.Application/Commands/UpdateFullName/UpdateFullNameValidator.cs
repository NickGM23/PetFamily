

using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Application.Commands.UpdateFullName
{
    public class UpdateFullNameValidator : AbstractValidator<UpdateFullNameCommand>
    {
        public UpdateFullNameValidator()
        {
            RuleFor(u => new { u.FullNameDto.LastName, u.FullNameDto.FirstName, u.FullNameDto.Patronymic })
                .MustBeValueObject(u => FullName.Create(u.LastName, u.FirstName, u.Patronymic));
        }
    }
}
