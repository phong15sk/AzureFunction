using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using CompanyAzureProject.Model;
using System.Data.SqlClient;

namespace RESTApiWithAzureFunction
{
   
    public static class CompanyAzureMain
    {
        public static String sqlConnectionString = "Server=tcp:dbcompany.database.windows.net,1433;Initial Catalog=Employee;Persist Security Info=False;User ID=phong;Password=Truyen15sk@gmail;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

       [FunctionName("CreateEmployee")]
        public static async Task<IActionResult> CreateEmployee(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "employee")] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<CreateEmployeeModel>(requestBody);
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    var query = $"INSERT INTO [Employee] (Name,CreatedOn,Position) VALUES('{input.Name}', '{input.CreatedOn}' , '{input.Position}')";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return new BadRequestResult();
            }
            return new OkResult();
        }

        [FunctionName("GetEmployee")]
        public static async Task<IActionResult> GetEmployee(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employee")] HttpRequest req, ILogger log)
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    var query = @"Select * from Employee";
                    SqlCommand command = new SqlCommand(query, connection);
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        EmployeeModel task = new EmployeeModel()
                        {
                            Id = (int)reader["Id"],
                            Name = reader["Name"].ToString(),
                            CreatedOn = (DateTime)reader["CreatedOn"],
                            Position = reader["Position"].ToString()
                        };
                        employeeList.Add(task);
                    }
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
            if (employeeList.Count > 0)
            {
                return new OkObjectResult(employeeList);
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [FunctionName("GetEmployeekById")]
        public static IActionResult GetEmployeeById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employee/{id}")] HttpRequest req, ILogger log, int id)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    var query = @"Select * from Employee Where Id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dt);
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
            if (dt.Rows.Count == 0)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(dt);
        }

        [FunctionName("DeleteEmployee")]
        public static IActionResult DeleteEmployee(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "employee/{id}")] HttpRequest req, ILogger log, int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    var query = @"Delete from Employee Where Id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
                return new BadRequestResult();
            }
            return new OkResult();
        }

        [FunctionName("UpdateEmployee")]
        public static async Task<IActionResult> UpdateEmployee(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "employee/{id}")] HttpRequest req, ILogger log, int id)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<UpdateEmployeeModel>(requestBody);
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    var query = @"Update Employee Set Name = @Name , Position = @Position Where Id = @Id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Name", input.Name);
                    command.Parameters.AddWithValue("@Position", input.Position);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
            return new OkResult();
        }
    }
}