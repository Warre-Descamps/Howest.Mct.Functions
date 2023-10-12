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

namespace Howest.Mct.Functions;

public static class PostRegistrations
{
    [FunctionName("PostRegistrations")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/registrations")] HttpRequest req, ILogger log)
    {
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var request = JsonConvert.DeserializeObject<RegistrationRequest>(requestBody);

        var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand("INSERT INTO [dbo].[Registrations]([Id], [LastName], [FirstName], [Email], [Zipcode], [Age], [IsFirstTimer]) VALUES(@id, @lastName, @firstName, @email, @zipcode, @age, @isFirstTimer)",
            connection);
        command.Parameters.AddWithValue("@id", Guid.NewGuid());
        command.Parameters.AddWithValue("@lastName", request.LastName);
        command.Parameters.AddWithValue("@firstName", request.FirstName);
        command.Parameters.AddWithValue("@email", request.Email);
        command.Parameters.AddWithValue("@zipcode", request.Zipcode);
        command.Parameters.AddWithValue("@age", request.Age);
        command.Parameters.AddWithValue("@isFirstTimer", request.IsFirstTimer);

        await command.ExecuteNonQueryAsync();

        await connection.CloseAsync();

        return new OkResult();
    }
}