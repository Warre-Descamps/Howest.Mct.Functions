using System.Linq;
using System.Threading.Tasks;
using Howest.Mct.Functions.CosmosDb.Helper;
using Howest.Mct.Functions.CosmosDb.Tasks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Howest.Mct.Functions.CosmosDb.Tasks;

public static class GetLocations
{
    [FunctionName("GetLocations")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "locations")] HttpRequest req, ILogger log)
    {
        var container = CosmosHelper.GetContainer("taskmetadata");
        
        var taskLocations = await container.GetItems<TaskMetadata>(t => t.Type == "LOCATION");

        return new OkObjectResult(taskLocations);
    }
}