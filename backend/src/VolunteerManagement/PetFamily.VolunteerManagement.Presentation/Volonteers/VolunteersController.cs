using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.Core.Abstractions;
using PetFamily.Core.Dtos;
using PetFamily.Core.Models;
using PetFamily.VolunteerManagement.Application.Commands.CreateVolunteer;
using PetFamily.VolunteerManagement.Application.Commands.AddPet;
using PetFamily.VolunteerManagement.Application.Commands.Delete;
using PetFamily.VolunteerManagement.Application.Commands.DeletePet;
using PetFamily.VolunteerManagement.Application.Commands.SetMainPetPhoto;
using PetFamily.VolunteerManagement.Application.Commands.RemovePhotosFromPet;
using PetFamily.VolunteerManagement.Application.Commands.UpdateMainInfo;
using PetFamily.VolunteerManagement.Application.Commands.UpdatePetStatus;
using PetFamily.VolunteerManagement.Application.Commands.UpdateRequisites;
using PetFamily.VolunteerManagement.Application.Commands.UpdatePet;
using PetFamily.VolunteerManagement.Application.Commands.UpdateSocialNetworks;
using PetFamily.VolunteerManagement.Application.Commands.UploadFilesToPet;
using PetFamily.VolunteerManagement.Application.Queries.GetVolunteer;
using PetFamily.VolunteerManagement.Application.Queries.GetVolunteersWithPagination;
using PetFamily.VolunteerManagement.Presentation.Volonteers.Requests;
using Microsoft.AspNetCore.Http;
using PetFamily.VolunteerManagement.Presentation.Processors;
using Microsoft.AspNetCore.Authorization;

namespace PetFamily.VolunteerManagement.Presentation.Volonteers
{
    public class VolunteersController : ApplicationController
    {
        private const string BUCKET_NAME = "photos";

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetById(
            [FromRoute] Guid id,
            [FromServices] IQueryHandler<VolunteerDto, GetVolunteerQuery> handler,
            CancellationToken cancellationToken = default)
        {
            var query = new GetVolunteerQuery(id);

            var result = await handler.Handle(query, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [Authorize]
        [HttpPut("{id:guid}/pet/{petId:guid}")]
        public async Task<ActionResult<Guid>> UpdatePet(
        [FromRoute] Guid id,
        [FromRoute] Guid petId,
        [FromBody] UpdatePetRequest request,
        [FromServices] UpdatePetHandler handler,
        CancellationToken cancellationToken = default)
        {
            var command = request.ToCommand(id, petId);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [Authorize]
        [HttpDelete("{id:guid}/pet/photos")]
        public async Task<ActionResult> RemovePhotosFromPet(
            [FromRoute] Guid id,
            [FromForm] RemovePhotosFromPetRequest request,
            [FromServices] RemovePhotosFromPetHandler handler,
            CancellationToken cancellationToken
        )
        {
            var result = await handler.Handle(request.ToCommand(id, BUCKET_NAME), cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [Authorize]
        [HttpPut("{id:guid}/pet/{petId:guid}/status")]
        public async Task<ActionResult<Guid>> UpdatePetStatus(
            [FromRoute] Guid id,
            [FromRoute] Guid petId,
            [FromBody] UpdatePetStatusRequest request,
            [FromServices] UpdatePetStatusHandler handler,
            CancellationToken cancellationToken = default)
        {
            var command = request.ToCommand(id, petId);

            var result = await handler.Handle(command, cancellationToken);

            return Ok(result.Value);
        }

        [Authorize]
        [HttpDelete("{id:guid}/pets/{petId:guid}")]
        public async Task<ActionResult<Guid>> DeletePet(
            [FromRoute] Guid id,
            [FromRoute] Guid petId,
            [FromServices] ICommandHandler<Guid, DeletePetCommand> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new DeletePetCommand(id, petId);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [Authorize]
        [HttpDelete("{id:guid}/pets/{petId:guid}/force")]
        public async Task<ActionResult<Guid>> ForceDeletePet(
            [FromRoute] Guid id,
            [FromRoute] Guid petId,
            [FromServices] ICommandHandler<Guid, RemovePhotosFromPetCommand> handler,
            CancellationToken cancellationToken = default)
        {
            var command = new RemovePhotosFromPetCommand(id, petId, BUCKET_NAME);

            var result = await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [Authorize]
        [HttpPatch("{volunteerId:guid}/pet/main-file")]
        public async Task<ActionResult> SetMainPetPhoto(
            [FromRoute] Guid volunteerId,
            [FromBody] SetMainPetPhotoRequest request,
            [FromServices] SetMainPetPhotoHandler handler,
            CancellationToken token)
        {
            var result = await handler.Handle(request.ToCommand(volunteerId), token);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}
