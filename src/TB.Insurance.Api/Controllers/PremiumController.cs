using Microsoft.AspNetCore.Mvc;
using TB.Insurance.Application.Premiums;
using TB.Insurance.Domain.Pricing;

namespace TB.Insurance.Api.Controllers;

[ApiController]
[Route("api/premium")]
[Produces("application/json")]
public sealed class PremiumController(GetPremiumHandler getPremium) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<GetPremiumResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<GetPremiumResponse>> GetPremium(
        [FromQuery] Guid employeeId,
        [FromQuery] PricingModel pricingModel,
        [FromQuery] ProratingMethod prorataMethod,
        CancellationToken cancellationToken)
    {
        var response = await getPremium.HandleAsync(
            new GetPremiumRequest(employeeId, pricingModel, prorataMethod),
            cancellationToken);

        return Ok(response);
    }
}
