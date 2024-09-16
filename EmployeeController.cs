using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace dotnet_api_code_challenge
{
    public class EmployeeController
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly EmployeeRepository _repository;
        private readonly EmployeeCustomService _sqlService;

        public EmployeeController(ILogger<EmployeeController> logger, EmployeeRepository repository, EmployeeCustomService sqlService)
        {
            _logger = logger;
            _repository = repository;
            _sqlService = sqlService;
        }

        [Function("GetAllEmployees")]
        public async Task<HttpResponseData> GetAllEmployees(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "employees")] HttpRequestData req)
        {
            var employees = await _repository.GetAllEmployeesAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(employees);
            return response;
        }

        [Function("GetEmployeeById")]
        public async Task<HttpResponseData> GetEmployeeById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "employees/{id}")] HttpRequestData req, int id)
        {
            var employee = await _repository.GetEmployeeByIdAsync(id);

            var response = employee != null
                ? req.CreateResponse(HttpStatusCode.OK)
                : req.CreateResponse(HttpStatusCode.NotFound);

            if (employee != null)
            {
                await response.WriteAsJsonAsync(employee);
            }

            return response;
        }

        [Function("AddEmployee")]
        public async Task<HttpResponseData> AddEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "employees")] HttpRequestData req)
        {
            var employee = await JsonSerializer.DeserializeAsync<Employee>(req.Body);

            if (employee == null)
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("Invalid employee data.");
                return badRequestResponse;
            }

            var createdEmployee = await _repository.AddEmployeeAsync(employee);

            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add("Location", $"/api/employees/{createdEmployee.Id}");
            await response.WriteAsJsonAsync(createdEmployee);

            return response;
        }

        [Function("UpdateEmployee")]
        public async Task<HttpResponseData> UpdateEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "employees/{id}")] HttpRequestData req, int id)
        {
            var employee = await JsonSerializer.DeserializeAsync<Employee>(req.Body);

            if (employee == null || employee.Id != id)
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("Invalid employee data.");
                return badRequestResponse;
            }

            var updatedEmployee = await _repository.UpdateEmployeeAsync(employee);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(updatedEmployee);

            return response;
        }

        [Function("DeleteEmployee")]
        public async Task<HttpResponseData> DeleteEmployee(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "employees/{id}")] HttpRequestData req, int id)
        {
            await _repository.DeleteEmployeeAsync(id);

            var response = req.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }

        [Function("GetEmployeesHiredInLastNYears")]
        public async Task<HttpResponseData> GetEmployeesHiredInLastNYears(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "employees/{numberOfYears}/hired")] HttpRequestData req, int numberOfYears = 1)
        {
            var employees = await _sqlService.GetEmployeesHiredInLastNYears(numberOfYears);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(employees);
            return response;
        }

        [Function("GetEmployeesByAgeRange")]
        public async Task<HttpResponseData> GetEmployeesByAgeRange(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "employees/{range}/age")] HttpRequestData req, string range = "20|30")
        {
            var ageRange = range.Split("|").ToArray().Select(x => int.Parse(x)).ToList();
            var employees = await _sqlService.GetEmployeesByAgeRange(ageRange.FirstOrDefault(), ageRange.Last());

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(employees);
            return response;
        }

        [Function("GetEmployeesByBirthdayMonth")]
        public async Task<HttpResponseData> GetEmployeesByBirthdayMonth(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "employees/{month}/birthday")] HttpRequestData req, int month = 1) // Defaults to January
        {
            var employees = await _sqlService.GetEmployeesByBirthdayMonth(month);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(employees);
            return response;
        }

        [Function("GetEmployeesWhoReportsTo")]
        public async Task<HttpResponseData> GetEmployeesWhoReportsTo(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "employees/{employeeId}/reportsTo")] HttpRequestData req, int employeeId)
        {
            var employees = await _sqlService.GetEmployeesWhoReportsTo(employeeId);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(employees);
            return response;
        }
    }
}
