using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Howest.Mct.Functions.CosmosDb.Helper;
using Howest.Mct.Functions.CosmosDb.Tasks.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Howest.Mct.Functions.CosmosDb.Tasks;

public static class GetTags
{
    [FunctionName("GetTags")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tags")] HttpRequest req, ILogger log)
    {
        var container = CosmosHelper.GetContainer(container: "taskmetadata");

        var taskTags = await container.GetItems<TaskMetadata>("SELECT * FROM c WHERE c.type = 'TAG'").ToListAsync();
        
        return new OkObjectResult(taskTags);
    }
}