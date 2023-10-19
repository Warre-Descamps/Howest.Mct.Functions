using System.Linq;
using System.Threading.Tasks;
using Howest.Mct.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Howest.Mct.Functions;

public class GetRegistrationsV2
{
    [FunctionName("GetRegistrationsV2")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v2/registrations")] HttpRequest req, ILogger log)
    {
        var registrations = await RegistrationsHelper.GetRegistrationsAsync().ToListAsync();

        return new OkObjectResult(registrations);
    }
}