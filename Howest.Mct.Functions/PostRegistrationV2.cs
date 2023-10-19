using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Howest.Mct.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Howest.Mct.Functions;

public class PostRegistrationV2
{
    [FunctionName("PostRegistrationV2")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v2/registrations")] HttpRequest req, ILogger log)
    {
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var request = JsonConvert.DeserializeObject<RegistrationRequest>(requestBody);
        
        var partitionKey = "registrations";
        var connectionString = "";
        var tableClient = new TableClient(connectionString, partitionKey);
        await tableClient.CreateIfNotExistsAsync();
        
        var rowKey = Guid.NewGuid();

        var entity = new TableEntity(partitionKey, rowKey.ToString())
        {
            { "age", request.Age },
            { "email", request.Email },
            { "firstname", request.FirstName },
            { "lastname", request.LastName },
            { "zipcode", request.Zipcode },
            { "isfirsttimer", request.IsFirstTimer },
            { "id", rowKey.ToString() },
        };

        await tableClient.AddEntityAsync(entity);

        var result = new RegistrationResult
        {
            Id = rowKey,
            LastName = request.LastName,
            FirstName = request.FirstName,
            Email = request.Email,
            Zipcode = request.Zipcode,
            Age = request.Age,
            IsFirstTimer = request.IsFirstTimer
        };

        return new OkObjectResult(result);
    }
}