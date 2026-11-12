using TB.Insurance.Domain.Common;
using TB.Insurance.Domain.Employees;
using TB.Insurance.Domain.Policies;

namespace TB.Insurance.UnitTests.Employees;

public class EmployeeTests
{
    [Fact]
    public void ANewEmployeeExposesTheDataItWasCreatedWith()
    {
        var added = new DateOnly(2026, 3, 1);
        var employee = new Employee(
            "Ana",
            "Diaz",
            new DateOnly(1990, 6, 15),
            Gender.Female,
            "ana.diaz@tb.com",
            TestPeriods.FullYear(),
            added);

        Assert.Equal("Ana Diaz", employee.FullName);
        Assert.Equal("ana.diaz@tb.com", employee.Email);
        Assert.Equal(Gender.Female, employee.Gender);
        Assert.Equal(added, employee.MemberAddedOn);
    }

    [Fact]
    public void EveryEmployeeGetsItsOwnIdentifier()
    {
        var first = TestEmployees.CreateAged(30);
        var second = TestEmployees.CreateAged(30);

        Assert.NotEqual(Guid.Empty, first.Id);
        Assert.NotEqual(first.Id, second.Id);
    }

    [Theory]
    [InlineData(2026, 3, 1, 35)]
    [InlineData(2026, 6, 15, 36)]
    [InlineData(2026, 6, 16, 36)]
    public void TheAgeIsTakenOnTheAdditionDate(int year, int month, int day, int expected)
    {
        var added = new DateOnly(year, month, day);
        var employee = TestEmployees.Create(new DateOnly(1990, 6, 15), Gender.Male, added);

        Assert.Equal(expected, employee.AgeOnAddition);
    }

    [Fact]
    public void NamesAndEmailAreTrimmed()
    {
        var employee = new Employee(
            "  Ana  ",
            "  Diaz  ",
            new DateOnly(1990, 6, 15),
            Gender.Female,
            "  ana.diaz@tb.com  ",
            TestPeriods.FullYear(),
            new DateOnly(2026, 1, 1));

        Assert.Equal("Ana", employee.FirstName);
        Assert.Equal("Diaz", employee.LastName);
        Assert.Equal("ana.diaz@tb.com", employee.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void AnEmptyNameIsRejected(string value)
    {
        Assert.Throws<DomainException>(() => New(value));
    }

    [Theory]
    [InlineData("sin-arroba")]
    [InlineData("@dominio.com")]
    [InlineData("usuario@")]
    public void AMalformedEmailIsRejected(string email)
    {
        Assert.Throws<DomainException>(() => new Employee(
            "Ana",
            "Diaz",
            new DateOnly(1990, 1, 1),
            Gender.Female,
            email,
            TestPeriods.FullYear(),
            new DateOnly(2026, 1, 1)));
    }

    [Fact]
    public void ANameIsTrimmedButNotLengthLimited()
    {
        var longName = new string('a', 101);
        var employee = New(longName);

        Assert.Equal(longName, employee.FirstName);
    }

    [Fact]
    public void TheAdditionDateMustFallInsideThePolicyPeriod()
    {
        Assert.Throws<DomainException>(() => new Employee(
            "Ana",
            "Diaz",
            new DateOnly(1990, 1, 1),
            Gender.Female,
            "ana@tb.com",
            new PolicyPeriod(new DateOnly(2026, 1, 1), new DateOnly(2026, 12, 31)),
            new DateOnly(2027, 5, 1)));
    }

    [Fact]
    public void TheDateOfBirthMustBeEarlierThanTheAdditionDate()
    {
        Assert.Throws<DomainException>(() => new Employee(
            "Ana",
            "Diaz",
            new DateOnly(2026, 6, 1),
            Gender.Female,
            "ana@tb.com",
            TestPeriods.FullYear(),
            new DateOnly(2026, 3, 1)));
    }

    [Fact]
    public void AnUndefinedGenderIsRejected()
    {
        Assert.Throws<DomainException>(() => new Employee(
            "Ana",
            "Diaz",
            new DateOnly(1990, 1, 1),
            (Gender)99,
            "ana@tb.com",
            TestPeriods.FullYear(),
            new DateOnly(2026, 1, 1)));
    }

    private static Employee New(string firstName) => new(
        firstName,
        "Diaz",
        new DateOnly(1990, 1, 1),
        Gender.Female,
        "ana@tb.com",
        TestPeriods.FullYear(),
        new DateOnly(2026, 1, 1));
}
