using Microsoft.AspNetCore.Mvc;

namespace PetFamily.Framework
{
    [ApiController]
    [Route("[controller]")]
    public abstract class ApplicationController : ControllerBase
    {
    }
}
