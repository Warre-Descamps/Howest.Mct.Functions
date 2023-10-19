using System;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Howest.Mct.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Logging;

namespace Howest.Mct.Functions;

public static class Export
{
    [FunctionName("Export")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v2/registrations/export")] HttpRequest req, ILogger log)
    {
        var registrations = await RegistrationsHelper.GetRegistrationsAsync().ToListAsync();

        var fileName = CsvHelper.WriteCsv(registrations);

        await UploadAsync(fileName);

        return new OkObjectResult(fileName);
    }

    private static async Task UploadAsync(string fileName)
    {
        const string containerName = "csv";
        
        var blobContainerClient = new BlobServiceClient(VariableHelper.StorageAccount).GetBlobContainerClient(containerName);
        await blobContainerClient.CreateIfNotExistsAsync();

        var blobClient = blobContainerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileName);
    }
}