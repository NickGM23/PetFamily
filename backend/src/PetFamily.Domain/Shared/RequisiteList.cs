using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared
{
    public record RequisiteList
    {
        private const int MIN_REQUISITES_COUNT = 1;

        public RequisiteList()
        {
        }

        public IReadOnlyList<Requisite> Requisites { get; }

        public RequisiteList(IEnumerable<Requisite> requisites)
        {
            Requisites = requisites.ToList();
        }

        public static Result<RequisiteList, Error> Create(IEnumerable<Requisite> value)
        {
            value = value.ToList();

            if (value.Count() < MIN_REQUISITES_COUNT)
                return Errors.General.InvalidCount(MIN_REQUISITES_COUNT, nameof(Requisites).ToLower());

            return new RequisiteList(value);
        }
    }
}
