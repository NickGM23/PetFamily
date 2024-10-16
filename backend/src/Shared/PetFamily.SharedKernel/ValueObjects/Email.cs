
using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace PetFamily.SharedKernel.ValueObjects
{
    public record Email
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Result<Email, Error> Create(string value)
        {
            if (!Regex.IsMatch(value, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                return Errors.General.WrongEmail(value);
            }

            return new Email(value);
        }
    }
}
