
using PetFamily.Application.Files.Get.GetFile;

namespace PetFamily.Application.Files.Get.GetFiles
{
    public record GetFilesCommand(IEnumerable<GetFileCommand> GetFileCommands);
}
