using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Howest.Mct.Functions;

public class HelloWorld
{
    [FunctionName("HelloWorld")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hello-world/{name}")] HttpRequest req,
        ILogger log, string name)
    {
        return new OkObjectResult($"Hello World {name}");
    }
}