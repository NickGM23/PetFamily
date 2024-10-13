namespace PetFamily.Core.Dtos
{
    public record SpeciesDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = default!;

        public string Description { get; init; } = default!;

        public IEnumerable<BreedDto> Breeds { get; init; } = default!;
    }
}
