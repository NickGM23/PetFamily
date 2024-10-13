
using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace PetFamily.SharedKernel.ValueObjects
{
    public record PhoneNumber
    {
        private PhoneNumber(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<PhoneNumber, Error> Create(string value)
        {
            if (!Regex.IsMatch(value, @"^[\+]?3?[\s]?8?[\s]?\(?0\d{2}?\)?[\s]?\d{3}[\s|-]?\d{2}[\s|-]?\d{2}$"))
            {
                return Errors.General.WrongPhoneNumber(value);
            }

            return new PhoneNumber(value);
        }
    }
}
