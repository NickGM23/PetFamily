using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Species.Requests;
using PetFamily.API.Controllers.Volonteers.Requests;
using PetFamily.API.Extensions;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;
using PetFamily.Application.Models;
using PetFamily.Application.Species.AddBreed;
using PetFamily.Application.Species.CreateSpecies;
using PetFamily.Application.Species.Delete;
using PetFamily.Application.Species.Queries.GetBreedsWithPagination;
using PetFamily.Application.Species.Queries.GetSpeciesWithPanination;

namespace PetFamily.API.Controllers.Species
{
    public class SpeciesController : ApplicationController
    {
        [HttpPost]
        public async Task<ActionResult> Create(
            [FromServices] CreateSpeciesHandler handler,
            [FromBody] CreateSpeciesRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand();

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPost("{id:guid}/breed")]
        [ProducesResponseType(typeof(AddPetRequest), StatusCodes.Status200OK)]
        public async Task<ActionResult> AddBreed(
            [FromRoute] Guid id,
            [FromBody] AddBreedRequest request,
            [FromServices] AddBreedHandler handler,
            CancellationToken cancellationToken = default)
        {
            var command = request.ToCommand(id);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(
            [FromServices] DeleteSpeciesHandler handler,
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var command = new DeleteSpeciesCommand(id);
            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<ActionResult> Get(
            [FromQuery] GetSpeciesWithPaginationRequest request,
            [FromServices] IQueryHandler<PagedList<SpeciesDto>, GetSpeciesWithPaginationQuery> handler,
            CancellationToken cancellationToken = default)
        {
            var query = request.ToQuery();

            var result = await handler.Handle(query, cancellationToken);
            return Ok(result.Value);
        }

        [HttpGet("{id::guid}/breeds")]
        public async Task<ActionResult> GetBreeds(
            [FromRoute] Guid id,
            [FromQuery] GetBreedsWithPaginationRequest request,
            [FromServices] IQueryHandler<PagedList<BreedDto>, GetBreedsWithPaginationQuery> handler,
            CancellationToken cancellationToken = default)
        {
            var query = request.ToQuery(id);

            var result = await handler.Handle(query, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}
