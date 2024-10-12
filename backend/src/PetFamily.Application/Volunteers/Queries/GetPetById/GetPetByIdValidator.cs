using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Queries.GetPetById
{
    public class GetPetByIdValidator : AbstractValidator<GetPetByIdQuery>
    {
        public GetPetByIdValidator()
        {
            RuleFor(p => p.PetId)
                .NotEmpty().WithError(Errors.General.ValueIsRequired("Pet id"));
        }
    }
}
