
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models
{
    public record PetPhoto
    {
        public string Path { get; }
        public bool IsMain { get; }

        private PetPhoto(string path, bool isMain)
        {
            Path = path;
            IsMain = isMain;
        }

        public static Result<PetPhoto, Error> Create(string path, bool isMain)
        {
            if (string.IsNullOrWhiteSpace(path) || path.Length > Constants.MAX_HIGH_TEXT_LENGTH)
                return Errors.General.ValueIsInvalid("Path");

            return new PetPhoto(path, isMain);
        }

    }
}
