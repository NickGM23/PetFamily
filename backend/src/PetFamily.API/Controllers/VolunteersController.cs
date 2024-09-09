using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.Application.Volunteers.CreateVolunteer;

namespace PetFamily.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VolunteersController : ControllerBase
    {
        [HttpGet]
        public string Get() 
        {
            return "Test";
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(
            [FromServices] CreateVolunteerHandler createVolunteerHandler,
            [FromBody] CreateVolunteerRequest createVolunteerRequest,
            CancellationToken cancellationToken)
        {
            var result = await createVolunteerHandler.Handle(createVolunteerRequest, cancellationToken);
            
            if (result.IsFailure)
                return result.Error.ToResponse();

            return new ObjectResult(result.Value) { StatusCode = 201};
        }
    }
}
