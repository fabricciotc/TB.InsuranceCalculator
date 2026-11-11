using Microsoft.EntityFrameworkCore;
using TB.Insurance.Domain.Common;
using TB.Insurance.Domain.Pricing;
using TB.Insurance.Infrastructure.Conexion;

namespace TB.Insurance.Application.Premiums;

public sealed class GetPremiumHandler(InsuranceDbContext context)
{
    public async Task<GetPremiumResponse> HandleAsync(
        GetPremiumRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var employee = await context.Employees
            .FirstOrDefaultAsync(employee => employee.Id == request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException($"Employee '{request.EmployeeId}' was not found.");

        var premium = PremiumCalculator.Calculate(employee, request.PricingModel, request.ProratingMethod);

        return new GetPremiumResponse(
            premium.EmployeeId,
            employee.FullName,
            employee.AgeOnAddition,
            premium.PricingModel,
            premium.ProratingMethod,
            premium.FullPremium,
            premium.ProratedPremium);
    }
}
