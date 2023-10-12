using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Howest.Mct.Functions;

public class GetDays
{
    [FunctionName("GetDays")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "days")] HttpRequest req,
        ILogger log)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
            
        var command =  new SqlCommand("SELECT [DagVanDeWeek] FROM (SELECT [TijdstipDag], [DagVanDeWeek] FROM [dbo].[Bezoekers] WHERE [TijdstipDag] < 7)", connection);

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