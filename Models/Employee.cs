using Newtonsoft.Json;
using System;

namespace EmployeeTimeTracking.Models
{
    public class Employee
    {
        public string Id { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;

        [JsonProperty("StarTimeUtc")]  // Match the JSON key
        public DateTime? StartTimeUtc { get; set; }

        public DateTime? EndTimeUtc { get; set; }

        public string EntryNotes { get; set; } = string.Empty;

        public double TotalTimeWorked { get; set; } = 0.0;

        public void CalculateTotalTimeWorked()
        {
            if (StartTimeUtc.HasValue && EndTimeUtc.HasValue)
            {
                TotalTimeWorked += Math.Round((EndTimeUtc.Value - StartTimeUtc.Value).TotalHours, 2);
            }
        }
    }
}
