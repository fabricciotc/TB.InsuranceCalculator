using Microsoft.EntityFrameworkCore;
using TB.Insurance.Domain.Employees;

namespace TB.Insurance.Infrastructure.Conexion;

public sealed class InsuranceDbContext(DbContextOptions<InsuranceDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>()
            .Property(employee => employee.Gender)
            .HasConversion<string>();
    }
}
