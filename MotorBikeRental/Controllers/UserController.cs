using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MotorBikeRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("Test")]
        public async Task <IActionResult> Test()
        {
            return Ok();
        }
    }
}
