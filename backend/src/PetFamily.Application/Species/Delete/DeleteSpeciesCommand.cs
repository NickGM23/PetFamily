﻿
using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Species.Delete
{
    public record DeleteSpeciesCommand(Guid SpeciesId) : ICommand;
}
