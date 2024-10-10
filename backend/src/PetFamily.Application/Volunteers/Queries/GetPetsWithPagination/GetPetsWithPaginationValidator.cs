
using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Queries.GetPetsWithPagination
{
    public class GetPetsWithPaginationValidator : AbstractValidator<GetPetsWithPaginationQuery>
    {
        public GetPetsWithPaginationValidator()
        {
            RuleFor(v => v.Page)
                .GreaterThanOrEqualTo(1)
                .WithError(Errors.General.ValueIsInvalid("Page"));

            RuleFor(v => v.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithError(Errors.General.ValueIsInvalid("Page size"));
        }
    }
}
