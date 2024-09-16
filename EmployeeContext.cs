using System;
using Microsoft.EntityFrameworkCore;

namespace dotnet_api_code_challenge;

public class EmployeeContext : DbContext
{
    public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, Name = "Jane Smith", Position = "Project Manager", Salary = 95000, DateOfHire = DateTime.UtcNow.AddMonths(-9), BirthDate = new DateTime(1995, 7, 21) },
            new Employee { Id = 2, Name = "Walter Page", Position = "Technical Lead", Salary = 95000, DateOfHire = DateTime.UtcNow.AddMonths(-9), BirthDate = new DateTime(1991, 12, 30), ReportsTo = 1 },
            new Employee { Id = 3, Name = "John Doe", Position = "Software Engineer", Salary = 80000, DateOfHire = DateTime.UtcNow.AddMonths(-15), BirthDate = new DateTime(2000, 4, 6), ReportsTo = 2 },
            new Employee { Id = 4, Name = "Devi Kotaru", Position = "QA Engineer", Salary = 95000, DateOfHire = DateTime.UtcNow.AddMonths(-9), BirthDate = new DateTime(1985, 1, 15), ReportsTo = 2 },
            new Employee { Id = 5, Name = "Jorge Ray", Position = "DevOps Engineer", Salary = 95000, DateOfHire = DateTime.UtcNow.AddMonths(-9), BirthDate = new DateTime(1999, 5, 23), ReportsTo = 2 }
        );
    }
}
