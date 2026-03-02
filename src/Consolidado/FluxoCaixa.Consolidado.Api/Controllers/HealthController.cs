using Microsoft.AspNetCore.Mvc;

namespace FluxoCaixa.Consolidado.Api.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "healthy", service = "consolidado-api", timestamp = DateTime.UtcNow });
}
