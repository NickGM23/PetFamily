
using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdatePetStatus
{
    public class UpdatePetStatusValidator : AbstractValidator<UpdatePetStatusCommand>
    {
        public UpdatePetStatusValidator()
        {
            RuleFor(p => p.Status).NotEmpty()
                .WithError(Errors.General.ValueIsRequired("help status"));

            RuleFor(p => p.Status).Must(s => Constants.PERMITTED_HELP_STATUSES_FROM_VOLUNTEER.Contains(s))
                .WithError(Errors.General.ValueIsInvalid("help status"));
        }
    }
}
