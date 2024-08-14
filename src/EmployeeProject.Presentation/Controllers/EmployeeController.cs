using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeProject.Core.Interface;
using EmployeeProject.Core.Models;
using Microsoft.AspNetCore.Mvc;
using EmployeeProject.Application.Interface;

namespace EmployeeProject.Presentation.Controllers;

public class EmployeeController(ITimeWorkedService timeWorkedService, IChartGenerator chartGenerator) : Controller
{
    private readonly ITimeWorkedService _timeWorkedService = timeWorkedService;
    private readonly IChartGenerator _chartGenerator = chartGenerator;

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

        if (!aggregatedEmployees.Any())
        {
            Console.WriteLine("No data to generate pie chart.");
        }
        else
        {
            Console.WriteLine("Data for pie chart:");
            foreach (var emp in aggregatedEmployees)
            {
                Console.WriteLine($"{emp.EmployeeName}: {emp.TotalTimeWorkedInHours} hours");
            }
        }

        // Generate the pie chart
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "EmployeeTimeWorkedChart.png");
        _chartGenerator.GeneratePieChart(aggregatedEmployees, filePath);

        return View("Index", aggregatedEmployees);
    }
}

