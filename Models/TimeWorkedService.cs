using System.Text.Json;

namespace EmployeeProject.Models;

public class TimeWorkedService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<List<EmployeeInfo>> GetTimeEntriesAsync()
    {
        //API
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

