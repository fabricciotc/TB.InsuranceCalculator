using TB.Insurance.Domain.Employees;
using TB.Insurance.Domain.Policies;
using TB.Insurance.Infrastructure;

namespace TB.Insurance.Application.Employees;

public sealed class SaveEmployeeHandler(InsuranceDbContext context, TimeProvider timeProvider)
{
    public async Task<SaveEmployeeResponse> HandleAsync(
        SaveEmployeeRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var coverage = new PolicyPeriod(request.PolicyStartDate, request.PolicyEndDate);

        var employee = new Employee(
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Gender,
            request.Email,
            coverage,
            request.MemberAddedOn,
            timeProvider.GetUtcNow());

        await context.Employees.AddAsync(employee, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new SaveEmployeeResponse(employee.Id);
    }
}
