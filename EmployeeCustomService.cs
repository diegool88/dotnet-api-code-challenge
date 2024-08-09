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

    public async Task<IEnumerable<Employee>> GetEmployeesHiredInLastNYears(int numberOfYears)
    {
        return await _context.Employees
        .Where(e => e.DateOfHire >= DateTime.UtcNow.AddYears(-1 * numberOfYears))
        .OrderByDescending(e => e.DateOfHire)
        .ToListAsync();
    }
}
