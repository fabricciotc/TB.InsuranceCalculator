using TB.Insurance.Domain.Common;
using TB.Insurance.Domain.Employees;
using TB.Insurance.Domain.Pricing;

namespace TB.Insurance.UnitTests.Pricing;

public class PricingRulesTests
{
    [Fact]
    public void FlatRate_AlwaysReturnsTheConfiguredAnnualPremium()
    {
        var employee = TestEmployees.CreateAged(35);

        var premium = PricingRules.FullPremium(employee, PricingModel.FlatRate, TestRates.Default);

        Assert.Equal(1_000m, premium);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(9, 900)]
    [InlineData(10, 2_000)]
    [InlineData(19, 3_800)]
    [InlineData(20, 6_000)]
    [InlineData(29, 8_700)]
    [InlineData(35, 14_000)]
    [InlineData(45, 22_500)]
    public void AgeRated_AppliesTheBandMultiplierOfTheAge(int age, decimal expected)
    {
        Assert.Equal(expected, PricingRules.AgeRated(age, TestRates.Default));
    }

    [Fact]
    public void AgeRated_UsesTheAgeAtAdmission()
    {
        var employee = TestEmployees.CreateAged(35);

        Assert.Equal(14_000m, PricingRules.FullPremium(employee, PricingModel.AgeRated, TestRates.Default));
    }

    [Fact]
    public void AgeRated_RejectsAgesOutsideTheSupportedRange()
    {
        Assert.Throws<DomainException>(() => PricingRules.AgeRated(-1, TestRates.Default));
        Assert.Throws<DomainException>(() => PricingRules.AgeRated(121, TestRates.Default));
    }

    [Theory]
    [InlineData(17, Gender.Female, 3_400)]
    [InlineData(10, Gender.Female, 2_000)]
    [InlineData(18, Gender.Female, 5_400)]
    [InlineData(35, Gender.Female, 21_000)]
    [InlineData(18, Gender.Male, 3_600)]
    [InlineData(35, Gender.Male, 14_000)]
    public void GenderAgeRated_LoadsFemalesOfLegalAgeByTheConfiguredFactor(
        int age,
        Gender gender,
        decimal expected)
    {
        Assert.Equal(expected, PricingRules.GenderAgeRated(age, gender, TestRates.Default));
    }

    [Fact]
    public void GenderAgeRated_MatchesAgeRatedForMales()
    {
        var ageRated = PricingRules.AgeRated(40, TestRates.Default);
        var genderAgeRated = PricingRules.GenderAgeRated(40, Gender.Male, TestRates.Default);

        Assert.Equal(ageRated, genderAgeRated);
    }
}
