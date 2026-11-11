using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TB.Insurance.Application.Configuration;
using TB.Insurance.Domain.Common;
using TB.Insurance.Domain.Pricing;
using TB.Insurance.Infrastructure;

namespace TB.Insurance.Application.Premiums;

public sealed class GetPremiumHandler(InsuranceDbContext context, IOptions<PremiumSettings> settings)
{
    public async Task<GetPremiumResponse> HandleAsync(
        GetPremiumRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var employee = await context.Employees
            .SingleOrDefaultAsync(employee => employee.Id == request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException($"Employee '{request.EmployeeId}' was not found.");

        var premium = PremiumCalculator.Calculate(
            employee,
            request.PricingModel,
            request.ProratingMethod,
            settings.Value.ToRates());

        return new GetPremiumResponse(employee.FullName,
            employee.AgeOnAddition,
            premium.PricingModel,
            premium.ProratingMethod,
            premium.FullPremium,
            premium.ProratedPremium);
    }
}
