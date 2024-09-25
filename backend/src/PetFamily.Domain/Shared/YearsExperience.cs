using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared
{
    public record YearsExperience
    {
        public int Value { get; }

        private YearsExperience(int value)
        {
            Value = value;
        }

        public static Result<YearsExperience, Error> Create(int value)
        {
            if (int.IsNegative(value) || value > 50)
            {
                return Result.Failure<YearsExperience, Error>(Errors.General.ValueIsInvalid(value.ToString(), "yearsOfExperience"));
            }

            return new YearsExperience(value);
        }
    }
}

