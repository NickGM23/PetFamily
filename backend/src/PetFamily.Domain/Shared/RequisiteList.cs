namespace PetFamily.Domain.Shared
{
    public record RequisiteList
    {
        public IReadOnlyList<Requisite> Requisites { get; } = null!;

        public RequisiteList(IEnumerable<Requisite> requisites)
        {
            Requisites = requisites.ToList();
        }
    }
}
