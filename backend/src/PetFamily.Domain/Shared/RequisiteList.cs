namespace PetFamily.Domain.Shared
{
    public class RequisiteList
    {
        private readonly List<Requisite> _requisites = [];

        public IReadOnlyList<Requisite> Requisites => _requisites;

        public void AddRequisite(Requisite requisite)
        {
            _requisites.Add(requisite);
        }
    }
}
