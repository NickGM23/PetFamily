
using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.SharedKernel;

namespace PetFamily.Core.Validation
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
            this IRuleBuilder<T, TElement> ruleBuilder,
            Func<TElement, Result<TValueObject, Error>> factoryMethod)
        {
            return ruleBuilder.Custom((value, context) =>
            {
                Result<TValueObject, Error> result = factoryMethod(value);

                if (result.IsSuccess)
                    return;

                context.AddFailure(result.Error.Serialize());
            });
        }

        public static IRuleBuilderOptionsConditions<T, string> MustBeAllowedExtension<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            IEnumerable<string> allowedExtensions)
        {
            return ruleBuilder.Custom((path, context) =>
            {
                var extension = Path.GetExtension(path);

                var isAllowedExtension = allowedExtensions.Contains(extension);

                if (isAllowedExtension == false)
                    context.AddFailure(Error.Validation("file.path", "Extension is invalid", "path")
                        .Serialize());
            });
        }

        public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule, Error error)
        {
            return rule.WithMessage(error.Serialize());
        }
    }
}
