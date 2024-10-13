
using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Application.Commands.DeletePet
{
    public class DeletePetCommandValidator : AbstractValidator<DeletePetCommand>
    {
        public DeletePetCommandValidator()
        {
            RuleFor(v => v.VolunteerId).NotEmpty()
                .WithError(Errors.General.ValueIsRequired("Volunteer id"));

            RuleFor(v => v.PetId).NotEmpty()
                .WithError(Errors.General.ValueIsRequired("Pet id"));

        }
    }
}
