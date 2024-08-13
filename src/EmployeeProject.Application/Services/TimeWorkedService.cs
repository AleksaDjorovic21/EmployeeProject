using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EmployeeProject.Core.Interface;
using EmployeeProject.Core.Models;

namespace EmployeeProject.Application.Services;

public class TimeWorkedService(HttpClient httpClient) : ITimeWorkedService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<List<EmployeeInfo>> GetTimeEntriesAsync()
    {
        // API
        string requestUrl = $"https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";

        try
        {
            // GET request
            var response = await _httpClient.GetStringAsync(requestUrl);

            // Deserialize JSON
            var entries = JsonSerializer.Deserialize<List<EmployeeInfo>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            // Handle null
            return entries ?? new List<EmployeeInfo>();
        }
        catch
        {
            return new List<EmployeeInfo>();
        }
    }
}



