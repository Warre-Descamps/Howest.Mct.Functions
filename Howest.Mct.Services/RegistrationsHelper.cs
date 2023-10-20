using Azure.Data.Tables;
using Azure.Identity;
using Howest.Mct.Models;

namespace Howest.Mct.Services;

public static class RegistrationsHelper
{
    public static async IAsyncEnumerable<RegistrationResult> GetRegistrationsAsync()
    {
        var partitionKey = "registrations";
        var tableClient = new TableClient(new Uri("https://stawd.table.core.windows.net"), partitionKey, new DefaultAzureCredential());

        var query = tableClient.QueryAsync<TableEntity>(filter: $"PartitionKey eq '{partitionKey}'");

        await foreach (var registration in query)
        {
            var stringId = registration["id"].ToString();
            var lastName = registration["lastname"].ToString();
            var firstName = registration["firstname"].ToString();
            var email = registration["email"].ToString();
            if (new[] {lastName, firstName, email}.Any(s => s is null) || !Guid.TryParse(stringId, out var id))
                continue;

            yield return new RegistrationResult
            {
                Id = id,
                LastName = lastName!,
                FirstName = firstName!,
                Email = email!,
                Zipcode = Convert.ToInt32(registration["zipcode"]),
                Age = Convert.ToInt32(registration["age"]),
                IsFirstTimer = Convert.ToBoolean(registration["isFirstTimer"])
            };
        }
    }
}