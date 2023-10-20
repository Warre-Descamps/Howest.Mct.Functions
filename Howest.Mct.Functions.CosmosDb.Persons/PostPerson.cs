using System;
using System.Threading.Tasks;
using Howest.Mct.Functions.CosmosDb.Helper;
using Howest.Mct.Functions.CosmosDb.Persons.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Howest.Mct.Functions.CosmosDb.Persons;

public static class PostPerson
{
    [FunctionName("PostPerson")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "persons")] HttpRequest req, ILogger log)
    {
        var person = JsonConvert.DeserializeObject<Person>(await req.ReadAsStringAsync());
        var container = CosmosHelper.GetContainer();
        
        person.Id = Guid.NewGuid();
        await container.CreateItemAsync(person, new PartitionKey(person.Id.ToString()));
        
        return new OkObjectResult(person);
    }
}