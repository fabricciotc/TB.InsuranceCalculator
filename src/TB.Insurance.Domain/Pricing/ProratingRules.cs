using TB.Insurance.Domain.Common;
using TB.Insurance.Domain.Policies;

namespace TB.Insurance.Domain.Pricing;

public static class ProratingRules
{
    public const int DaysInYear = 365;
    public const int MonthsInYear = 12;

    public static decimal Prorate(
        decimal fullPremium,
        ProratingMethod method,
        PolicyPeriod coverage,
        DateOnly memberAddedOn)
    {
        ArgumentNullException.ThrowIfNull(coverage);

        if (!coverage.Contains(memberAddedOn))
        {
            throw new DomainException("The member addition date must fall inside the policy period.");
        }

        return method switch
        {
            ProratingMethod.ByDays => fullPremium / DaysInYear * RemainingDays(coverage, memberAddedOn),
            ProratingMethod.ByMonths => fullPremium / MonthsInYear * RemainingMonths(coverage, memberAddedOn),
            _ => throw new DomainException($"The prorating method '{method}' is not supported.")
        };
    }

    public static int RemainingDays(PolicyPeriod coverage, DateOnly memberAddedOn) =>
        coverage.EndDate.DayNumber - memberAddedOn.DayNumber + 1;

    public static int RemainingMonths(PolicyPeriod coverage, DateOnly memberAddedOn)
    {
        var months = (coverage.EndDate.Year - memberAddedOn.Year) * 12
                     + coverage.EndDate.Month - memberAddedOn.Month;

        if (coverage.EndDate.Day >= memberAddedOn.Day)
        {
            months++;
        }

        return Math.Clamp(months, 1, MonthsInYear);
    }
}
