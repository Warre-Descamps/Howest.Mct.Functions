using Howest.Mct.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;

namespace Howest.Mct.Functions;

public static class GetVisitors
{
    [FunctionName("GetVisitors")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "visitors/{day}")] HttpRequest req,
        ILogger log, string day)
    {
        var token = await new DefaultAzureCredential().GetTokenAsync(new TokenRequestContext(new[] { "https://database.windows.net/.default" }));
        
        var connection = new SqlConnection();
        connection.AccessToken = token.Token;
        await connection.OpenAsync();

        var command = new SqlCommand("SELECT [TijdstipDag], [AantalBezoekers], [DagVanDeWeek] FROM [dbo].[Bezoekers] WHERE [DagVanDeWeek] = @day", connection);
        command.Parameters.AddWithValue("@day", day);

        var days = new List<VisitorResult>();
        await using (var reader = await command.ExecuteReaderAsync())
        {
            while (reader.Read())
            {
                var dbDay = reader["DagVanDeWeek"].ToString();
                if (dbDay is null)
                    continue;

                days.Add(new VisitorResult
                {
                    Time = Convert.ToInt32(reader["TijdstipDag"]),
                    Amount = Convert.ToInt32(reader["AantalBezoekers"]),
                    Day = day
                });
            }
        }

        await connection.CloseAsync();

        return new OkObjectResult(days);
    }
}