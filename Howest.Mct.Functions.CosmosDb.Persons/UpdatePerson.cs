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

public static class UpdatePerson
{
    [FunctionName("UpdatePerson")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "persons/{id}")] HttpRequest req, ILogger log, Guid id)
    {
        var person = JsonConvert.DeserializeObject<Person>(await req.ReadAsStringAsync());
        person.Id = id;
        var container = CosmosHelper.GetContainer();

        //await CosmosHelper.GetItem<Person>(container, p => p.Id == id);

        await container.ReplaceItemAsync(person, id.ToString(), new PartitionKey(id.ToString()));

        return new OkObjectResult(person);
    }
}