using TB.Insurance.Domain.Pricing;

namespace TB.Insurance.Application.Premiums;

public sealed record GetPremiumRequest(
    Guid EmployeeId,
    PricingModel PricingModel,
    ProratingMethod ProratingMethod);

public sealed record GetPremiumResponse(
    string FullName,
    int Age,
    PricingModel PricingModel,
    ProratingMethod ProratingMethod,
    decimal FullPremium,
    decimal ProratedPremium);
