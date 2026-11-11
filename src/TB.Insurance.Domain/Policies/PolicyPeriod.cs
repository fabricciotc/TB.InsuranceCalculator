using TB.Insurance.Domain.Common;

namespace TB.Insurance.Domain.Policies;

public sealed record PolicyPeriod
{
    public DateOnly StartDate { get; }

    public DateOnly EndDate { get; }

    public PolicyPeriod(DateOnly startDate, DateOnly endDate)
    {
        if (endDate <= startDate)
            throw new DomainException("The policy end date must be later than the policy start date.");

        StartDate = startDate;
        EndDate = endDate;
    }

    public bool Contains(DateOnly date) => date >= StartDate && date <= EndDate;
}
