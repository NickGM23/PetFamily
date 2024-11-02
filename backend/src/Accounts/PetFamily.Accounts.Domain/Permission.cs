
namespace PetFamily.Accounts.Domain
{
    public class Permission
    {
        public Guid Id { get; init; }

        public string Code { get; set; } = string.Empty;

        public List<RolePermission> RolePermissions { get; set; } = [];
    }
}
