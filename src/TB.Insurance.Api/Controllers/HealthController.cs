using Microsoft.AspNetCore.Mvc;

namespace TB.Insurance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<HealthResponse>(StatusCodes.Status200OK)]
    public ActionResult<HealthResponse> Get() =>
        Ok(new HealthResponse("healthy", "TB.InsuranceCalculator", DateTimeOffset.UtcNow));
}

public sealed record HealthResponse(string Status, string Service, DateTimeOffset Timestamp);
