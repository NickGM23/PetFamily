using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;

namespace PetFamily.Accounts.Application.Commands.UpdateFullName
{
    public record UpdateFullNameCommand(Guid UserId, FullNameDto FullNameDto) : ICommand;
}
