using TB.Insurance.Domain.Employees;
using TB.Insurance.Domain.Policies;

namespace TB.Insurance.UnitTests;

internal static class TestEmployees
{
    public static Employee Create(
        DateOnly dateOfBirth,
        Gender gender = Gender.Male,
        DateOnly? memberAddedOn = null)
    {
        return new Employee(
            "Ana",
            "Diaz",
            dateOfBirth,
            gender,
            "ana.diaz@tb.com",
            TestPeriods.FullYear(),
            memberAddedOn ?? new DateOnly(2026, 1, 1));
    }

    public static Employee CreateAged(int age, Gender gender = Gender.Male, DateOnly? memberAddedOn = null)
    {
        var added = memberAddedOn ?? new DateOnly(2026, 1, 1);

        return Create(added.AddYears(-age), gender, added);
    }
}
