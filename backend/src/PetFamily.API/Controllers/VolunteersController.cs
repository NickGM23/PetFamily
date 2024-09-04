using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<Guid>> Create([FromServices] CreateVolunteerHandler createVolunteerHandler,
                                                     [FromBody] CreateVolunteerRequest createVolunteerRequest,
                                                     CancellationToken cancellationToken = default)
        {
            var result = await createVolunteerHandler.Handle(createVolunteerRequest, cancellationToken);
            if (result.IsFailure)
                return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}
