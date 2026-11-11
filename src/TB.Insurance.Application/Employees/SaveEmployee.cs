using TB.Insurance.Domain.Employees;

namespace TB.Insurance.Application.Employees;

public sealed record SaveEmployeeRequest(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    Gender Gender,
    string Email,
    DateOnly PolicyStartDate,
    DateOnly PolicyEndDate,
    DateOnly MemberAddedOn);

public sealed record SaveEmployeeResponse(Guid EmployeeId);
