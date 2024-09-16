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

    public async Task<IEnumerable<Employee>> GetEmployeesByAgeRange(int startAge, int endAge)
    {
        return await _context.Employees
        .Where(p => GetAgeFromBirthDate(p.BirthDate) >= startAge && GetAgeFromBirthDate(p.BirthDate) <= endAge)
        .OrderBy(e => e.BirthDate)
        .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetEmployeesByBirthdayMonth(int month)
    {
        return await _context.Employees
        .Where(e => e.BirthDate.Month.Equals(month))
        .OrderBy(e => e.BirthDate)
        .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetEmployeesWhoReportsTo(int employeeId)
    {
        return await _context.Employees
        .Where(e => e.ReportsTo != null && e.ReportsTo.Equals(employeeId))
        .OrderBy(e => e.Id)
        .ToListAsync();
    }

    private static int GetAgeFromBirthDate(DateTime birthDate){
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        return age;
    }
}
