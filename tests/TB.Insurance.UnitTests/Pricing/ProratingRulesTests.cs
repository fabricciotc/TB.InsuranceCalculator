using TB.Insurance.Domain.Common;
using TB.Insurance.Domain.Pricing;

namespace TB.Insurance.UnitTests.Pricing;

public class ProratingRulesTests
{
    [Fact]
    public void RemainingDays_CountsInclusiveFromTheAdditionDate()
    {
        var coverage = TestPeriods.FullYear();

        Assert.Equal(306, ProratingRules.RemainingDays(coverage, new DateOnly(2026, 3, 1)));
        Assert.Equal(365, ProratingRules.RemainingDays(coverage, new DateOnly(2026, 1, 1)));
        Assert.Equal(1, ProratingRules.RemainingDays(coverage, new DateOnly(2026, 12, 31)));
    }

    [Fact]
    public void RemainingMonths_CountsFromTheAdditionDate()
    {
        var coverage = TestPeriods.FullYear();

        Assert.Equal(10, ProratingRules.RemainingMonths(coverage, new DateOnly(2026, 3, 15)));
        Assert.Equal(12, ProratingRules.RemainingMonths(coverage, new DateOnly(2026, 1, 1)));
        Assert.Equal(10, ProratingRules.RemainingMonths(coverage, new DateOnly(2026, 3, 1)));
    }

    [Fact]
    public void ByDays_ProratesOverTheRemainingDaysOfTheYear()
    {
        var premium = ProratingRules.Prorate(
            1_000m,
            ProratingMethod.ByDays,
            TestPeriods.FullYear(),
            new DateOnly(2026, 3, 1));

        Assert.Equal(838.356m, premium, 3);
    }

    [Fact]
    public void ByMonths_ProratesOverTheRemainingMonthsOfTheYear()
    {
        var premium = ProratingRules.Prorate(
            1_000m,
            ProratingMethod.ByMonths,
            TestPeriods.FullYear(),
            new DateOnly(2026, 3, 15));

        Assert.Equal(833.333m, premium, 3);
    }

    [Fact]
    public void ProratingAWholeYearReturnsTheFullPremium()
    {
        var coverage = TestPeriods.FullYear();
        var firstDay = new DateOnly(2026, 1, 1);

        Assert.Equal(
            1_000m,
            ProratingRules.Prorate(1_000m, ProratingMethod.ByDays, coverage, firstDay));
        Assert.Equal(
            1_000m,
            ProratingRules.Prorate(1_000m, ProratingMethod.ByMonths, coverage, firstDay));
    }

    [Fact]
    public void Prorate_RejectsAnAdditionDateOutsideThePolicyPeriod()
    {
        Assert.Throws<DomainException>(() => ProratingRules.Prorate(
            1_000m,
            ProratingMethod.ByDays,
            TestPeriods.FullYear(),
            new DateOnly(2027, 5, 1)));
    }
}
