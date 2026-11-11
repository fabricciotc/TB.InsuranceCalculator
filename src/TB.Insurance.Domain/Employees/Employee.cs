using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TB.Insurance.Domain.Common;
using TB.Insurance.Domain.Policies;

namespace TB.Insurance.Domain.Employees;

[Table("Employees")]
public sealed class Employee : Entity
{
    [Required]
    public string FirstName { get; private set; } = string.Empty;

    [Required]
    public string LastName { get; private set; } = string.Empty;

    [Required]
    public DateOnly DateOfBirth { get; private set; }

    [Required]
    public Gender Gender { get; private set; }

    [Required]
    public string Email { get; private set; } = string.Empty;

    [Required]
    public DateOnly PolicyStartDate { get; private set; }

    [Required]
    public DateOnly PolicyEndDate { get; private set; }

    [Required]
    public DateOnly MemberAddedOn { get; private set; }


    public string FullName => $"{FirstName} {LastName}".Trim();

    public PolicyPeriod Coverage => new(PolicyStartDate, PolicyEndDate);

    public int AgeOnAddition => AgeAt(MemberAddedOn);


    public Employee(
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        Gender gender,
        string email,
        PolicyPeriod coverage,
        DateOnly memberAddedOn)
    {
        Rename(firstName, lastName);
        ChangeEmail(email);
        ChangeGender(gender);
        AssignCoverage(coverage, memberAddedOn);
        ChangeDateOfBirth(dateOfBirth);
    }

    private void Rename(string firstName, string lastName)
    {
        FirstName = Required(firstName, nameof(firstName));
        LastName = Required(lastName, nameof(lastName));
    }

    private void ChangeEmail(string email)
    {
        var value = Required(email, nameof(email));
        var atIndex = value.IndexOf('@', StringComparison.Ordinal);

        if (atIndex <= 0 || atIndex == value.Length - 1)
            throw new DomainException("The email address is not valid.");

        Email = value;
    }

    private void ChangeGender(Gender gender)
    {
        if (!Enum.IsDefined(gender))
            throw new DomainException("The gender is not valid.");

        Gender = gender;
    }

    private void AssignCoverage(PolicyPeriod coverage, DateOnly memberAddedOn)
    {
        ArgumentNullException.ThrowIfNull(coverage);

        if (!coverage.Contains(memberAddedOn))
            throw new DomainException("The member addition date must fall inside the policy period.");

        PolicyStartDate = coverage.StartDate;
        PolicyEndDate = coverage.EndDate;
        MemberAddedOn = memberAddedOn;
    }

    private void ChangeDateOfBirth(DateOnly dateOfBirth)
    {
        if (dateOfBirth >= MemberAddedOn)
            throw new DomainException("The date of birth must be earlier than the member addition date.");

        DateOfBirth = dateOfBirth;
    }

    private int AgeAt(DateOnly date)
    {
        var age = date.Year - DateOfBirth.Year;

        if (date < DateOfBirth.AddYears(age))
            age--;

        return age;
    }

    private static string Required(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException($"'{paramName}' is required.");

        var trimmed = value.Trim();

        return trimmed;
    }
}
