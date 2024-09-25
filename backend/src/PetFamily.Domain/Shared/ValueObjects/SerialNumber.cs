using CSharpFunctionalExtensions;


namespace PetFamily.Domain.Shared.ValueObjects
{
    public record SerialNumber
    {
        public static SerialNumber First => new(1);

        public int Value { get; }

        private SerialNumber(int value)
        {
            Value = value;
        }

        public static Result<SerialNumber, Error> Create(int value)
        {
            if (value <= 0)
                return Errors.General.ValueIsInvalid("serial number");

            return new SerialNumber(value);
        }

        public static implicit operator int(SerialNumber serialNumber) =>
            serialNumber.Value;
    }
}