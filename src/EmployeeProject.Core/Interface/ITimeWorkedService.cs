using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeProject.Core.Models;

namespace EmployeeProject.Core.Interface;

public interface ITimeWorkedService
{
    Task<List<EmployeeInfo>> GetTimeEntriesAsync();
}
