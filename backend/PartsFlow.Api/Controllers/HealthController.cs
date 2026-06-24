using Microsoft.AspNetCore.Mvc;

namespace PartsFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "ok",
            app = "PartsFlow.Api"
        });
    }
}
