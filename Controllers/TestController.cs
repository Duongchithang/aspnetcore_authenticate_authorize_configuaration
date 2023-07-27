using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Controllers
{
    [Route("/api/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public IActionResult Test()
        {
            return Ok("Test success");
        }
    }
}
