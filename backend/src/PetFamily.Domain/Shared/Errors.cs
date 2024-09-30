
namespace PetFamily.Domain.Shared
{
    public class Errors
    {
        public static class General
        {
            public static Error ValueIsInvalid(string? name = null)
            {
                var label = name == null ? " " : $" '{name}' ";
                return Error.Validation("value.is.invalid", $"value{label}is invalid.");
            }
            public static Error ValueIsInvalid(string value, string invalidFieldName)
            {
                return Error.Validation("value.is.invalid", $"value '{value}' is invalid.", invalidFieldName);
            }

            public static Error NotFound(Guid? id = null)
            {
                var forId = id == null ? "" : $" for id '{id}'";
                return Error.NotFound("record.not.found", $"record not found{forId}.");
            }

            public static Error ValueIsRequired(string? name = null)
            {
                var label = name == null ? " " : $" '{name}' ";
                return Error.Validation("length.is.invalid", $"invalid{label}length.");
            }

            public static Error AlreadyExist(string? name = null)
            {
                var label = name == null ? " " : $" '{name}' ";
                return Error.Conflict("record.already.exist", $"record{label}already exist");
            }

            public static Error AlreadyExists(string name, string key, string value)
            {
                return Error.Conflict("record.already.exists", $"{name} already exists with {key + " = " + value}");
            }

            public static Error WrongEmail(string? email = null)
            {
                var label = email == null ? " " : $" '{email}' ";
                return Error.Validation("email.is.invalid", $"email{label}is invalid.");
            }

            public static Error WrongPhoneNumber(string? phoneNumber = null)
            {
                var label = phoneNumber == null ? " " : $" '{phoneNumber}' ";
                return Error.Validation("phone.is.invalid", $"phone number{label}is invalid.");
            }

            public static Error InvalidCount(int min, string? name = null, int? max = null)
            {
                var label = name == null ? "" : $" '{name}'";
                var forMaxLabel = max == null ? "" : $" and more than {max}";
                return Error.Validation("out.of.range", $"Value{label} can not be less than {min}{forMaxLabel}");
            }

            public static Error AlreadyUsed(Guid id)
            {
                return Error.Conflict("value.already.used", $"{id} is already used");
            }
        }
    }
}
