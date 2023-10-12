using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Howest.Mct.Models;

namespace Howest.Mct.Functions;

public class Addition
{
    [FunctionName("Addition")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "add/{a:int}/{b:int}")] HttpRequest req,
        ILogger log, int a, int b)
    {
        var result = new CalculatorResult
        {
            Result = (a + b).ToString(),
            Operator = "+"
        };

        return new OkObjectResult(result);
    }
}