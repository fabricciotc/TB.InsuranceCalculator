using TB.Insurance.Domain.Common;
using TB.Insurance.Domain.Employees;

namespace TB.Insurance.Domain.Pricing;

public static class PricingRules
{
    public const decimal FlatAnnualPremium = 1_000m;
    public const int AgeBandWidth = 10;
    public const int MaximumAge = 120;
    public const int FemaleLoadingAge = 18;
    public const decimal FemaleLoadingFactor = 1.5m;

    public static decimal FullPremium(Employee employee, PricingModel model)
    {
        ArgumentNullException.ThrowIfNull(employee);

        return model switch
        {
            PricingModel.FlatRate => FlatAnnualPremium,
            PricingModel.AgeRated => AgeRated(employee.AgeOnAddition),
            PricingModel.GenderAgeRated => GenderAgeRated(employee.AgeOnAddition, employee.Gender),
            _ => throw new DomainException($"The pricing model '{model}' is not supported.")
        };
    }

    public static decimal AgeRated(int age)
    {
        if (age is < 0 or > MaximumAge)
        {
            throw new DomainException("The member age is outside the supported range.");
        }

        var band = age / AgeBandWidth;

        return age * ((band + 1) * 100m);
    }

    public static decimal GenderAgeRated(int age, Gender gender)
    {
        var premium = AgeRated(age);

        return gender == Gender.Female && age >= FemaleLoadingAge
            ? premium * FemaleLoadingFactor
            : premium;
    }
}
