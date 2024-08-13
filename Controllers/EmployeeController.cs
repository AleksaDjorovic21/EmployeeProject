using EmployeeProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeProject.Controllers;

public class EmployeeController(TimeWorkedService timeWorkedService) : Controller
{
    private readonly TimeWorkedService _timeWorkedService = timeWorkedService;

    public async Task<IActionResult> Index()
    {
        var employees = await _timeWorkedService.GetTimeEntriesAsync();
        
        var aggregatedEmployees = employees
            .Where(e => !string.IsNullOrEmpty(e.EmployeeName)).Where(e => !string.IsNullOrEmpty(e.EmployeeName))
            .GroupBy(e => e.EmployeeName)
            .Select(g => new AggregatedEmployeeInfo
            {
                EmployeeName = g.Key,
                TotalTimeWorkedInHours = g.Sum(e => e.TotalTimeWorkedInHours)
            })
            .OrderByDescending(e => e.TotalTimeWorkedInHours)
            .ToList();

        return View("Index", aggregatedEmployees);
    }
}

