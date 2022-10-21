using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Azure.Samples
{
    public static class Cars
    {
        // Visit https://aka.ms/sqlbindingsinput to learn how to use this input binding
        [FunctionName("GetCars")]
        public static IActionResult RunGetCars(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "cars")] HttpRequest req,
            [Sql("SELECT * FROM dbo.Cars",
                CommandType = System.Data.CommandType.Text,
                ConnectionStringSetting = "SqlConnectionString")] IEnumerable<Object> result,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger with SQL Input Binding function processed a request.");

            return new OkObjectResult(result);
        }

        // Visit https://aka.ms/sqlbindingsoutput to learn how to use this output binding
        [FunctionName("AddCar")]
        public static async Task<ObjectResult> RunAddCar(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "cars")] HttpRequest req,
            [Sql("dbo.Cars", ConnectionStringSetting = "SqlConnectionString")] IAsyncCollector<Car> output,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger with SQL Output Binding function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            try {
                Car newCar = JsonConvert.DeserializeObject<Car>(requestBody);
                newCar.Id = Guid.NewGuid();
                await output.AddAsync(newCar);
                return new CreatedResult(req.Path, newCar);
            } catch (Exception ex) {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }

    public class Car
    {
        // make, model, year, color
        public Guid? Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
    }
}
