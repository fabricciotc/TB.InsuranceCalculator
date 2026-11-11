namespace TB.Insurance.Domain.Pricing;

public sealed record Premium(
    Guid EmployeeId,
    PricingModel PricingModel,
    ProratingMethod ProratingMethod,
    decimal FullPremium,
    decimal ProratedPremium);
