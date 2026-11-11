using TB.Insurance.Domain.Pricing;

namespace TB.Insurance.UnitTests;

internal static class TestRates
{
    public static readonly PremiumRates Default = new(
        FlatAnnualPremium: 1_000m,
        AgeBandMultiplier: 100m,
        AgeBandWidth: 10,
        MaximumAge: 120,
        FemaleLoadingAge: 18,
        FemaleLoadingFactor: 1.5m);
}
