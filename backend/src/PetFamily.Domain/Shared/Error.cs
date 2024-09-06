
namespace PetFamily.Domain.Shared
{
    public record Error
    {
        public string Code { get; }
        public string Message { get; }
        private ErrorType Type { get; }
        public string TypeError => Enum.GetName(typeof(ErrorType), Type)?.ToLower() ?? string.Empty;
        public string? InvalidField { get; }

        private Error(string code, string message, ErrorType type, string? invalidField = null)
        {
            Code = code;
            Message = message;
            Type = type;
            InvalidField = invalidField;
        }

        public static Error Validation(string code, string message, string? invalidField = null) =>
            new Error(code, message, ErrorType.Validation, invalidField);

        public static Error NotFound(string code, string message) =>
           new Error(code, message, ErrorType.NotFound);

        public static Error Failure(string code, string message) =>
           new Error(code, message, ErrorType.Failure);

        public static Error Conflict(string code, string message) =>
           new Error(code, message, ErrorType.Conflict);

    }

    public enum ErrorType
    {
        Validation,
        NotFound,
        Failure,
        Conflict
    }
}