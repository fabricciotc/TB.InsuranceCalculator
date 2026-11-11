using TB.Insurance.Domain.Common;
using TB.Insurance.Domain.Employees;

namespace TB.Insurance.Domain.Pricing;

public static class PricingRules
{
    public static decimal FullPremium(Employee employee, PricingModel model, PremiumRates rates)
    {
        ArgumentNullException.ThrowIfNull(employee);
        ArgumentNullException.ThrowIfNull(rates);

        return model switch
        {
            PricingModel.FlatRate => rates.FlatAnnualPremium,
            PricingModel.AgeRated => AgeRated(employee.AgeOnAddition, rates),
            PricingModel.GenderAgeRated => GenderAgeRated(employee.AgeOnAddition, employee.Gender, rates),
            _ => throw new DomainException($"The pricing model '{model}' is not supported.")
        };
    }

    public static decimal AgeRated(int age, PremiumRates rates)
    {
        if (age < 0 || age > rates.MaximumAge)
            throw new DomainException("The member age is outside the supported range.");

        var band = age / rates.AgeBandWidth;

        return age * ((band + 1) * rates.AgeBandMultiplier);
    }

    public static decimal GenderAgeRated(int age, Gender gender, PremiumRates rates)
    {
        var premium = AgeRated(age, rates);

        return gender == Gender.Female && age >= rates.FemaleLoadingAge
            ? premium * rates.FemaleLoadingFactor
            : premium;
    }
}
