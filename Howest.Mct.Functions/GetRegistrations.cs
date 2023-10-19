using Howest.Mct.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Howest.Mct.Services;

namespace Howest.Mct.Functions;

public static class GetRegistrations
{
    [FunctionName("GetRegistrations")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/registrations")] HttpRequest req, ILogger log)
    {
        try
        {
            var token = await new DefaultAzureCredential().GetTokenAsync(new TokenRequestContext(new[] { "https://database.windows.net/.default" }));
        
            var connection = new SqlConnection(VariableHelper.ConnectionString);
            connection.AccessToken = token.Token;
            await connection.OpenAsync();

            var command = new SqlCommand("SELECT [Id], [LastName], [FirstName], [Email], [Zipcode], [Age], [IsFirstTimer] FROM [dbo].[Registrations]", connection);

            var registrations = new List<RegistrationResult>();
            await using (var reader = await command.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    var stringId = reader["Id"].ToString();
                    var lastName = reader["LastName"].ToString();
                    var firstName = reader["FirstName"].ToString();
                    var email = reader["Email"].ToString();
                    if (new[] {lastName, firstName, email}.Any(s => s is null) || !Guid.TryParse(stringId, out var id))
                        continue;

                    registrations.Add(new RegistrationResult
                    {
                        Id = id,
                        LastName = lastName!,
                        FirstName = firstName!,
                        Email = email!,
                        Zipcode = Convert.ToInt32(reader["ZipCode"]),
                        Age = Convert.ToInt32(reader["Age"]),
                        IsFirstTimer = Convert.ToBoolean(reader["IsFirstTimer"])
                    });
                }
            }

            await connection.CloseAsync();

            return new OkObjectResult(registrations);
        }
        catch (Exception e)
        {
            return new ObjectResult(e.Message)
            {
                StatusCode = 500
            };
        }
    }
}