using Microsoft.AspNetCore.Mvc;
using TB.Insurance.Application.Premiums;
using TB.Insurance.Domain.Pricing;

namespace TB.Insurance.Api.Controllers;

[ApiController]
[Route("api/premium")]
public sealed class PremiumController(GetPremiumHandler getPremium) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetPremiumResponse>> GetPremium(
        [FromQuery] Guid employeeId,
        [FromQuery] PricingModel pricingModel,
        [FromQuery] ProratingMethod prorateMethod,
        CancellationToken cancellationToken)
    {
        var response = await getPremium.HandleAsync(
            new GetPremiumRequest(employeeId, pricingModel, prorateMethod),
            cancellationToken);

        return Ok(response);
    }
}
