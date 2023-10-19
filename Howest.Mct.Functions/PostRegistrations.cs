using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Howest.Mct.Models;
using System.Data.SqlClient;
using Azure.Core;
using Azure.Identity;
using Howest.Mct.Services;

namespace Howest.Mct.Functions;

public static class PostRegistrations
{
    [FunctionName("PostRegistrations")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/registrations")] HttpRequest req, ILogger log)
    {
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var request = JsonConvert.DeserializeObject<RegistrationRequest>(requestBody);

        var token = await new DefaultAzureCredential().GetTokenAsync(new TokenRequestContext(new[] { "https://database.windows.net/.default" }));
        
        var connection = new SqlConnection(VariableHelper.ConnectionString);
        connection.AccessToken = token.Token;
        await connection.OpenAsync();

        await using var command = new SqlCommand();
        command.Connection = connection;
        command.Parameters.Add(new SqlParameter("@id", Guid.NewGuid()));
        command.Parameters.Add(new SqlParameter("@lastName", request.LastName));
        command.Parameters.Add(new SqlParameter("@firstName", request.FirstName));
        command.Parameters.Add(new SqlParameter("@email", request.Email));
        command.Parameters.Add(new SqlParameter("@zipcode", request.Zipcode));
        command.Parameters.Add(new SqlParameter("@age", request.Age));
        command.Parameters.Add(new SqlParameter("@isFirstTimer", request.IsFirstTimer));
        command.CommandText =
            "INSERT INTO [dbo].[Registrations]([Id], [LastName], [FirstName], [Email], [Zipcode], [Age], [IsFirstTimer]) VALUES(@id, @lastName, @firstName, @email, @zipcode, @age, @isFirstTimer)";

        await command.ExecuteNonQueryAsync();

        await connection.CloseAsync();

        return new OkResult();
    }
}