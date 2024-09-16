using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Application.Volunteers.UpdateRequisites
{
    public class UpdateRequisitesCommandValidator : AbstractValidator<UpdateRequisitesCommand>
    {
        public UpdateRequisitesCommandValidator()
        {
            RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
            RuleForEach(u => u.Requisites)
                .MustBeValueObject(r => Requisite.Create(r.Name, r.Description));
        }
    }
}
