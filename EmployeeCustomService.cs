using System;
using Microsoft.EntityFrameworkCore;

namespace dotnet_api_code_challenge;

public class EmployeeCustomService
{
    private readonly EmployeeContext _context;

    public EmployeeCustomService(EmployeeContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetEmployeesHiredInLastYearAsync()
    {
        var query = "SELECT * FROM Employees WHERE DateOfHire >= @p0 ORDER BY DateOfHire DESC";
        return await _context.Employees.FromSqlRaw(query, DateTime.UtcNow.AddYears(-1)).ToListAsync();
    }
}
