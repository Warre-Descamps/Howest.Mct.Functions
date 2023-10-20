using System.Threading.Tasks;
using Howest.Mct.Functions.CosmosDb.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Howest.Mct.Functions.CosmosDb.Persons;

public static class DeletePerson
{
    [FunctionName("DeletePerson")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "persons/{id}")] HttpRequest req, ILogger log, string id)
    {
        var container = CosmosHelper.GetContainer();

        await container.DeleteItemAsync<string>(id, new PartitionKey(id));

        return new OkResult();
    }
}