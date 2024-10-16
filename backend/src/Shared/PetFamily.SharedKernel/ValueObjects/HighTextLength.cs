
using CSharpFunctionalExtensions;
using System.Runtime.CompilerServices;

namespace PetFamily.SharedKernel.ValueObjects
{
    public record HighTextLength
    {
        public string Value { get; }

        private HighTextLength(string value)
        {
            Value = value;
        }

        public static Result<HighTextLength, Error> Create(string value,
            [CallerArgumentExpression(nameof(value))] string? variableName = null)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > Constants.MAX_HIGH_TEXT_LENGTH)
                return Errors.General.ValueIsInvalid(variableName);

            return new HighTextLength(value);
        }

        public static implicit operator string(HighTextLength highTextLength) => highTextLength.Value;

        public static implicit operator HighTextLength(string s) => new(s);
    }
}
