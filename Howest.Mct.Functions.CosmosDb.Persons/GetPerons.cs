using System.Linq;
using System.Threading.Tasks;
using Howest.Mct.Functions.CosmosDb.Helper;
using Howest.Mct.Functions.CosmosDb.Persons.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Howest.Mct.Functions.CosmosDb.Persons;

public static class GetPerons
{
    [FunctionName("GetPerons")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "persons")] HttpRequest req, ILogger log)
    {
        var container = CosmosHelper.GetContainer();
        
        var persons = await CosmosHelper.GetItems<Person>(container).ToListAsync();
        
        return new OkObjectResult(persons);
    }
}