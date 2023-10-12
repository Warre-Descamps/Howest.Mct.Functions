using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Howest.Mct.Models;
using Newtonsoft.Json;

namespace Howest.Mct.Functions;

public class Calculator
{
    [FunctionName("Calculator")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "calculator")] HttpRequest req,
        ILogger log)
    {
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var request = JsonConvert.DeserializeObject<CalculationRequest>(requestBody);
        try
        {
            var result = new CalculatorResult
            {
                Operator = request.Operator,
                Result = new DataTable().Compute($"{request.A}{request.Operator}{request.B}", null)?.ToString() ?? string.Empty
            };
            return new OkObjectResult(result);
        }
        catch (Exception e)
        {
            var result = new CalculatorResult
            {
                Operator = request.Operator,
                Result = e.Message
            };
            return new BadRequestObjectResult(result);
        }
    }
}