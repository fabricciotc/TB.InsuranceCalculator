using TB.Insurance.Domain.Policies;

namespace TB.Insurance.UnitTests;

internal static class TestPeriods
{
    public static readonly DateOnly Start = new(2026, 1, 1);
    public static readonly DateOnly End = new(2026, 12, 31);

    public static PolicyPeriod FullYear() => new(Start, End);
}
