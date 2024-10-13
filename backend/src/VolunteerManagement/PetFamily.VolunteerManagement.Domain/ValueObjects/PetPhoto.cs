
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Domain.ValueObjects
{
    public record PetPhoto
    {
        public FilePath Path { get; }
        public bool IsMain { get; }

        private PetPhoto(FilePath path, bool isMain)
        {
            Path = path;
            IsMain = isMain;
        }

        public static Result<PetPhoto, Error> Create(FilePath path, bool isMain)
        {
            return new PetPhoto(path, isMain);
        }
    }
}
