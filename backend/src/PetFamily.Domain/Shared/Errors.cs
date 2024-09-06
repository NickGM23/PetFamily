
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

            public static Error NotFound(Guid? id = null)
            {
                var forId = id == null ? "" : $" for id '{id}'";
                return Error.Validation("record.not.found", $"record not found{forId}.");
            }

            public static Error ValueIsRequired(string? name = null)
            {
                var label = name == null ? " " : $" '{name}' ";
                return Error.Validation("length.is.invalid", $"invalid{label}length.");
            }
        }

        public static class Volunteer
        {
            public static Error AlreadyExist()
            {
                return Error.Validation("record.already.exist", $"Volunteer already exist");
            }

            public static Error WrongEmail(string? email = null)
            {
                var label = email == null ? " " : $" '{email}' ";
                return Error.Validation("email.is.invalid", $"Email{label}is invalid.");
            }
        }
    }
}
