using Microsoft.AspNetCore.Mvc;
using EmployeeTimeTracking.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EmployeeTimeTracking.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient _httpClient;

        public EmployeeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        [HttpGet("/Employee/Index")]
        public async Task<IActionResult> Index()
        {
            var apiUrl = "https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";
            var response = await _httpClient.GetStringAsync(apiUrl);
            var entries = JsonConvert.DeserializeObject<List<Employee>>(response);

            // Dictionary to accumulate time worked per employee
            var employeeWorkTime = new Dictionary<string, double>();

            foreach (var entry in entries)
            {
                // Ensure EmployeeName is not null
                if (!string.IsNullOrEmpty(entry.EmployeeName) && entry.StartTimeUtc.HasValue && entry.EndTimeUtc.HasValue)
                {
                    var duration = (entry.EndTimeUtc.Value - entry.StartTimeUtc.Value).TotalHours;
                    if (employeeWorkTime.ContainsKey(entry.EmployeeName))
                    {
                        employeeWorkTime[entry.EmployeeName] += duration;
                    }
                    else
                    {
                        employeeWorkTime[entry.EmployeeName] = duration;
                    }
                }
            }

            // Convert the dictionary back to a list of Employee objects with accumulated total time
            var employees = employeeWorkTime.Select(e => new Employee
            {
                EmployeeName = e.Key,
                TotalTimeWorked = Math.Round(e.Value, 0)  // Round to nearest hour for display
            })
            .OrderByDescending(e => e.TotalTimeWorked)
            .ToList();

            return View(employees);
        }




        // Optional: API endpoint to return the data as JSON
        [HttpGet("/api/Employee/GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            var apiUrl = "https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";
            var response = await _httpClient.GetStringAsync(apiUrl);
            var employees = JsonConvert.DeserializeObject<List<Employee>>(response);

            return Ok(employees);  // Returns the data as JSON for the API
        }
    }
}
