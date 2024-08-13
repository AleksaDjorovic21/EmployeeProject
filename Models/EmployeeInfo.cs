using System.Text.Json.Serialization;

namespace EmployeeProject.Models;

public class EmployeeInfo
{
    public string EmployeeName { get; set; } = string.Empty;
    
    [JsonPropertyName("StarTimeUtc")]
    public DateTime StartTimeUtc { get; set; }

    [JsonPropertyName("EndTimeUtc")]
    public DateTime EndTimeUtc { get; set; }

    // Calculated property
    public double TotalTimeWorkedInHours
    {
        get
        {
            if (EndTimeUtc < StartTimeUtc)
            {         
                return 0;
            }

            return (EndTimeUtc - StartTimeUtc).TotalHours;
        }
    }
}