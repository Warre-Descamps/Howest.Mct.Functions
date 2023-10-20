using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Howest.Mct.Services;

namespace Howest.Mct.Functions;

public static class GetDays
{
    [FunctionName("GetDays")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "days")] HttpRequest req,
        ILogger log)
    {
        var token = await new DefaultAzureCredential().GetTokenAsync(new TokenRequestContext(new[] { "https://database.windows.net/.default" }));
        
        var connection = new SqlConnection(VariableHelper.DatabaseUrl);
        connection.AccessToken = token.Token;
        await connection.OpenAsync();
            
        var command =  new SqlCommand("SELECT [DagVanDeWeek] FROM (SELECT [TijdstipDag], [DagVanDeWeek] FROM [dbo].[Bezoekers] WHERE [TijdstipDag] < 7) as TDDVDW", connection);

        var days = new List<string>();
        await using (var reader = await command.ExecuteReaderAsync())
        {
            while (reader.Read())
            {
                days.Add(reader["DagVanDeWeek"].ToString());
            }
        }
            
        await connection.CloseAsync();

        return new OkObjectResult(days);
    }
}