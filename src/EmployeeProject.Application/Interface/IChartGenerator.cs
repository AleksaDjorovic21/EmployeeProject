using System.Collections.Generic;
using EmployeeProject.Core.Models;

namespace EmployeeProject.Application.Interface;

public interface IChartGenerator
{
    void GeneratePieChart(List<AggregatedEmployeeInfo> data, string filePath);
}

