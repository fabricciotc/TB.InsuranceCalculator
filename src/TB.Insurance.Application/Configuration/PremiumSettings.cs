using TB.Insurance.Domain.Pricing;

namespace TB.Insurance.Application.Configuration;

public sealed class PremiumSettings
{
    public const string SectionName = "PremiumRates";

    public decimal FlatAnnualPremium { get; set; } = 1_000m;

    public decimal AgeBandMultiplier { get; set; } = 100m;

    public int AgeBandWidth { get; set; } = 10;

    public int MaximumAge { get; set; } = 120;

    public int FemaleLoadingAge { get; set; } = 18;

    public decimal FemaleLoadingFactor { get; set; } = 1.5m;

    public PremiumRates ToRates() => new(
        FlatAnnualPremium,
        AgeBandMultiplier,
        AgeBandWidth,
        MaximumAge,
        FemaleLoadingAge,
        FemaleLoadingFactor);
}
