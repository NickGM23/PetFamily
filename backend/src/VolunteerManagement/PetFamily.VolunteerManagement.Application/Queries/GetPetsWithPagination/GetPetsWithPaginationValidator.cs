
using FluentValidation;
using PetFamily.Core.Validation;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Application.Queries.GetPetsWithPagination
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
