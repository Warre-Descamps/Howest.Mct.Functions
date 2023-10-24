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
using Task = Howest.Mct.Functions.CosmosDb.Tasks.Models.Task;

namespace Howest.Mct.Functions.CosmosDb.Tasks;

public static class GetTasks
{
    [FunctionName("GetTasks")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tasks/{userId:Guid}")] HttpRequest req, ILogger log, Guid userId)
    {
        var container = CosmosHelper.GetContainer(container: "tasks");

        var tasks = await container.GetItems<Task>().ToListAsync();

        return new OkObjectResult(tasks);
    }
}