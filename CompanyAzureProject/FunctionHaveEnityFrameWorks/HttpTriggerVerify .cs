using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FunctionHaveEnityFrameWorks.Model;
using System.Linq;

namespace FunctionHaveEnityFrameWorks
{
    public class HttpTriggerVerify
    {
        private readonly EmployeeContext employeeContext;
        public HttpTriggerVerify(EmployeeContext employeeContext)
        {
            this.employeeContext = employeeContext;
        }
        [FunctionName("HttpTriggerVerify")]
        public IActionResult GetEmployees(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req, ILogger log)
        {
            string SqlConnection = Environment.GetEnvironmentVariable("SqlConnectionString");
            log.LogInformation("C# HTTP trigger function processed a request.");
            var employees = employeeContext.Employees.ToList();
            return new OkObjectResult(employees);
        }
        [FunctionName("SaveEmployee")]
        public async Task<ActionResult> SaveEmployeeAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req ,ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<Employee>(requestBody);
            await employeeContext.Employees.AddAsync(data);
            await employeeContext.SaveChangesAsync();
            return new OkResult();
        }
    }
}
