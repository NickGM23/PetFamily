using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetFamily.Framework;
using PetFamily.VolunteerManagement.Application.Queries.GetPetById;
using PetFamily.VolunteerManagement.Application.Queries.GetPetsWithPagination;
using PetFamily.VolunteerManagement.Presentation.Pets.Requests;

namespace PetFamily.VolunteerManagement.Presentation.Pets
{
    public class PetsController : ApplicationController
    {

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetPetsWithPagination(
            [FromQuery] GetPetsWithPaginationRequest request,
            [FromServices] GetPetsWithPaginationHandler handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.Handle(request.ToQuery(), cancellationToken);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }

        [HttpGet("{petId:guid}")]
        public async Task<ActionResult> GetPetById(
            [FromRoute] Guid petId,
            [FromServices] GetPetByIdHandler handler,
            CancellationToken token)
        {
            var result = await handler.Handle(new GetPetByIdQuery(petId), token);
            if (result.IsFailure)
                return result.Error.ToResponse();

            return Ok(result.Value);
        }
    }
}
