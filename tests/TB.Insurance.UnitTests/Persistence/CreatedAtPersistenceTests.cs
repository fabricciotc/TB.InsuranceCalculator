using Microsoft.EntityFrameworkCore;
using TB.Insurance.Domain.Employees;
using TB.Insurance.Infrastructure;

namespace TB.Insurance.UnitTests.Persistence;

public class CreatedAtPersistenceTests
{
    private static InsuranceDbContext NewContext(string database) =>
        new(new DbContextOptionsBuilder<InsuranceDbContext>()
            .UseInMemoryDatabase(database)
            .Options);

    [Fact]
    public async Task CreatedAtIsStoredOnInsert()
    {
        await using var context = NewContext(Guid.NewGuid().ToString());
        var employee = TestEmployees.CreateAged(35);
        var before = DateTimeOffset.UtcNow.AddSeconds(-1);

        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        Assert.InRange(employee.CreatedAt, before, DateTimeOffset.UtcNow.AddSeconds(1));
    }

    [Fact]
    public async Task CreatedAtSurvivesARoundTripToTheDatabase()
    {
        var database = Guid.NewGuid().ToString();
        var id = Guid.NewGuid();
        DateTimeOffset original;

        await using (var context = NewContext(database))
        {
            var employee = TestEmployees.CreateAged(35);
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            id = employee.Id;
            original = employee.CreatedAt;
        }

        await using (var context = NewContext(database))
        {
            var reloaded = await context.Employees.SingleAsync(e => e.Id == id);

            Assert.Equal(original, reloaded.CreatedAt);
        }
    }

    [Fact]
    public async Task TwoEmployeesCreatedTogetherKeepDistinctTimestamps()
    {
        await using var context = NewContext(Guid.NewGuid().ToString());
        var first = TestEmployees.CreateAged(30);
        var second = TestEmployees.CreateAged(40);

        context.Employees.AddRange(first, second);
        await context.SaveChangesAsync();

        var reloaded = await context.Employees.ToListAsync();

        Assert.All(reloaded, employee => Assert.NotEqual(default, employee.CreatedAt));
        Assert.All(reloaded, employee => Assert.Equal(DateTimeOffset.UtcNow.Offset, employee.CreatedAt.Offset));
    }

    [Fact]
    public async Task CreatedAtIsNotOverwrittenWhenTheEmployeeIsUpdated()
    {
        await using var context = NewContext(Guid.NewGuid().ToString());
        var employee = TestEmployees.CreateAged(35);
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        var original = employee.CreatedAt;

        employee.Rename("Ana Maria", "Diaz");
        await context.SaveChangesAsync();

        Assert.Equal(original, employee.CreatedAt);
    }
}
