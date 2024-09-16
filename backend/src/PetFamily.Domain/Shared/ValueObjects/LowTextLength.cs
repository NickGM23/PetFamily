
using CSharpFunctionalExtensions;
using System.Runtime.CompilerServices;

namespace PetFamily.Domain.Shared.ValueObjects
{
    public record LowTextLength
    {
        public string Value { get; }

        private LowTextLength(string value)
        {
            Value = value;
        }

        public static Result<LowTextLength, Error> Create(string value,
            [CallerArgumentExpression(nameof(value))] string? variableName = null)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_LOW_TEXT_LENGTH)
                return Errors.General.ValueIsInvalid(variableName);

            return new LowTextLength(value);
        }

        public static implicit operator string(LowTextLength lowTextLength) => lowTextLength.Value;

        public static implicit operator LowTextLength(string s) => new(s);
    }
}
