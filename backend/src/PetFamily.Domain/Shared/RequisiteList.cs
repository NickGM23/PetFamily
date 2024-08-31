namespace PetFamily.Domain.Shared
{
    public record RequisiteList
    {

        public IReadOnlyList<Requisite> Requisites { get; private set; } = null!;

    }
}
