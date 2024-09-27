using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Volonteers.Requests;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Dtos;
using PetFamily.Application.Models;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.Queries.GetVolunteersWithPagination;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdateRequisites;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;
using PetFamily.Application.Volunteers.UploadFilesToPet;

namespace PetFamily.API.Controllers.Volonteers
{
    public class VolunteersController : ApplicationController
    {
        private const string BUCKET_NAME = "photos";

        [HttpPost]
        public async Task<ActionResult> Create(
            [FromServices] CreateVolunteerHandler handler,
            [FromBody] CreateVolunteerRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand();

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPut("{id:guid}/main-info")]
        public async Task<ActionResult> UpdateMainInfo(
            [FromServices] UpdateVolunteerMainInfoHandler handler,
            [FromRoute] Guid id,
            [FromBody] UpdateVolunteerMainInfoRequest request,
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
            [FromServices] DeleteVolunteerHandler handler,
            [FromRoute] Guid id,
            CancellationToken cancellationToken = default)
        {
            var command = new DeleteVolunteerCommand(id);
            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPatch("{id:guid}/requisites")]
        public async Task<ActionResult<Guid>> UpdateRequisites(
            [FromRoute] Guid id,
            [FromBody] UpdateRequisitesRequest request,
            [FromServices] UpdateRequisitesHandler handler,
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand(id);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPatch("{id:guid}/social-networks")]
        public async Task<ActionResult<Guid>> UpdateSocialNetworks(
            [FromRoute] Guid id,
            [FromBody] UpdateSocialNetworksRequest request,
            [FromServices] UpdateSocialNetworksHandler handler,
            CancellationToken cancellationToken)
        {
            var command = request.ToCommand(id);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPost("{id:guid}/pet")]
        [ProducesResponseType(typeof(AddPetRequest), StatusCodes.Status200OK)]
        public async Task<ActionResult> AddPet(
            [FromRoute] Guid id,
            [FromBody] AddPetRequest request,
            [FromServices] AddPetHandler handler,
            CancellationToken cancellationToken = default)
        {
            var command = request.ToCommand(id);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpPost("{id:guid}/pet/{petId:guid}/files")]
        public async Task<ActionResult> UploadFilesToPet(
            [FromRoute] Guid id,
            [FromRoute] Guid petId,
            [FromForm] IFormFileCollection files,
            [FromServices] UploadFilesToPetHandler handler,
            CancellationToken cancellationToken = default)
        {
            await using var fileProcessor = new FormFileProcessor();
            var fileDtos = fileProcessor.Process(files);

            var command = new UploadFilesToPetCommand(id, petId, BUCKET_NAME, fileDtos);

            var result = await handler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpGet]
        public async Task<ActionResult> Get(
            [FromQuery] GetVolunteersWithPaginationRequest request,
            [FromServices] IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery> handler,
            CancellationToken cancellationToken = default)
        {
            var query = request.ToQuery();

            var result = await handler.Handle(query, cancellationToken);
            return Ok(result.Value);
        }
    }
}
