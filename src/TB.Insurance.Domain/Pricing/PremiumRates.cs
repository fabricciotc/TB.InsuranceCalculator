namespace TB.Insurance.Domain.Pricing;

public sealed record PremiumRates(
    decimal FlatAnnualPremium,
    decimal AgeBandMultiplier,
    int AgeBandWidth,
    int MaximumAge,
    int FemaleLoadingAge,
    decimal FemaleLoadingFactor);
