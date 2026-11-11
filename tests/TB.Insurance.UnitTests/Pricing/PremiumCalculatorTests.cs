using TB.Insurance.Domain.Common;
using TB.Insurance.Domain.Employees;
using TB.Insurance.Domain.Pricing;

namespace TB.Insurance.UnitTests.Pricing;

public class PremiumCalculatorTests
{
    [Fact]
    public void FlatRateByDays_ReproducesTheStatementExample()
    {
        var employee = TestEmployees.CreateAged(35, Gender.Female, new DateOnly(2026, 3, 1));

        var premium = PremiumCalculator.Calculate(
            employee,
            PricingModel.FlatRate,
            ProratingMethod.ByDays,
            TestRates.Default);

        Assert.Equal(1_000m, premium.FullPremium);
        Assert.Equal(838.36m, premium.ProratedPremium);
    }

    [Fact]
    public void FlatRateByMonths_ReproducesTheStatementExample()
    {
        var employee = TestEmployees.CreateAged(35, Gender.Female, new DateOnly(2026, 3, 15));

        var premium = PremiumCalculator.Calculate(
            employee,
            PricingModel.FlatRate,
            ProratingMethod.ByMonths,
            TestRates.Default);

        Assert.Equal(1_000m, premium.FullPremium);
        Assert.Equal(833.33m, premium.ProratedPremium);
    }

    [Fact]
    public void GenderAgeRatedByDays_AppliesTheFemaleLoading()
    {
        var employee = TestEmployees.CreateAged(35, Gender.Female, new DateOnly(2026, 3, 1));

        var premium = PremiumCalculator.Calculate(
            employee,
            PricingModel.GenderAgeRated,
            ProratingMethod.ByDays,
            TestRates.Default);

        Assert.Equal(21_000m, premium.FullPremium);
        Assert.Equal(17_605.48m, premium.ProratedPremium);
    }

    [Fact]
    public void AFullYearMemberPaysTheWholePremium()
    {
        var employee = TestEmployees.CreateAged(35, Gender.Male, new DateOnly(2026, 1, 1));

        var premium = PremiumCalculator.Calculate(
            employee,
            PricingModel.AgeRated,
            ProratingMethod.ByDays,
            TestRates.Default);

        Assert.Equal(premium.FullPremium, premium.ProratedPremium);
    }

    [Fact]
    public void TheResultEchoesTheRequestedModels()
    {
        var employee = TestEmployees.CreateAged(35);

        var premium = PremiumCalculator.Calculate(
            employee,
            PricingModel.AgeRated,
            ProratingMethod.ByMonths,
            TestRates.Default);

        Assert.Equal(employee.Id, premium.EmployeeId);
        Assert.Equal(PricingModel.AgeRated, premium.PricingModel);
        Assert.Equal(ProratingMethod.ByMonths, premium.ProratingMethod);
    }

    [Fact]
    public void TheProratedPremiumIsRoundedToTwoDecimals()
    {
        var employee = TestEmployees.CreateAged(20, Gender.Male, new DateOnly(2026, 5, 7));

        var premium = PremiumCalculator.Calculate(
            employee,
            PricingModel.AgeRated,
            ProratingMethod.ByDays,
            TestRates.Default);

        Assert.Equal(decimal.Round(premium.ProratedPremium, 2), premium.ProratedPremium);
    }

    [Fact]
    public void RejectsAnUnsupportedPricingModel()
    {
        var employee = TestEmployees.CreateAged(35);

        Assert.Throws<DomainException>(() => PremiumCalculator.Calculate(
            employee,
            (PricingModel)999,
            ProratingMethod.ByDays,
            TestRates.Default));
    }

    [Fact]
    public void RejectsAnUnsupportedProratingMethod()
    {
        var employee = TestEmployees.CreateAged(35);

        Assert.Throws<DomainException>(() => PremiumCalculator.Calculate(
            employee,
            PricingModel.FlatRate,
            (ProratingMethod)999,
            TestRates.Default));
    }
}
