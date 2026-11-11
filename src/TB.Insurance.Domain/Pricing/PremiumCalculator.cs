using TB.Insurance.Domain.Employees;

namespace TB.Insurance.Domain.Pricing;

public static class PremiumCalculator
{
    public static Premium Calculate(
        Employee employee,
        PricingModel pricingModel,
        ProratingMethod proratingMethod,
        PremiumRates rates)
    {
        ArgumentNullException.ThrowIfNull(employee);

        var fullPremium = PricingRules.FullPremium(employee, pricingModel, rates);
        var proratedPremium = ProratingRules.Prorate(fullPremium, proratingMethod, employee.Coverage, employee.MemberAddedOn);

        return new Premium(
            employee.Id,
            pricingModel,
            proratingMethod,
            fullPremium,
            decimal.Round(proratedPremium, 2, MidpointRounding.AwayFromZero));
    }
}
