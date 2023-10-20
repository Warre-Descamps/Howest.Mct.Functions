using System.Linq.Expressions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace Howest.Mct.Functions.CosmosDb.Helper;

public static class CosmosHelper
{
    private const string ConnectionString = "CosmosConnectionString";
    private const string DatabaseString = "CosmosDatabase";
    private const string ContainerString = "CosmosContainer";

    private static string? _cosmosConnectionString;
    private static string CosmosConnectionString => _cosmosConnectionString ??= Environment.GetEnvironmentVariable(ConnectionString)
                                                    ?? throw new ArgumentNullException($"{ConnectionString} is a required value in local.settings!");

    private static string? _database;
    private static string Database => _database ??= Environment.GetEnvironmentVariable(DatabaseString)
                                                    ?? throw new ArgumentNullException($"{DatabaseString} is a required value in local.settings!");

    private static string? _container;
    private static string Container => _container ??= Environment.GetEnvironmentVariable(ContainerString)
                                                      ?? throw new ArgumentNullException($"{ContainerString} is a required value in local.settings!");
  
    public static CosmosClient GetCosmosClient()
    {
        var options = new CosmosClientOptions()
        {
            ConnectionMode = ConnectionMode.Gateway
        };

        return new CosmosClient(CosmosConnectionString, options);
    }

    public static Container GetContainer(CosmosClient client)
    {
        return client.GetContainer(Database, Container);
    }

    public static Container GetContainer()
    {
        return GetContainer(GetCosmosClient());
    }

    public static IAsyncEnumerable<T> GetItems<T>(Container container)
        where T : class
    {
        return GetItems<T>(container, "SELECT * FROM c");
    }

    public static async IAsyncEnumerable<T> GetItems<T>(Container container, string query)
        where T: class
    {
        var iterator = container.GetItemQueryIterator<T>(query);
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            foreach (var item in response)
            {
                yield return item;
            }
        }
    }

    /*internal static async Task<T> GetItem<T>(Container container, Expression<Func<T, int, bool>> predicate)
        where T : class, new()
    {
        Queryable.Where<T>(predicate).ToQueryDefinition().QueryText
        
        return GetItem<T>(container, "SELECT * FROM c WHERE ")
    }*/

    internal static async Task<T> GetItem<T>(Container container, string query)
        where T: class, new()
    {
        var iterator = container.GetItemQueryIterator<T>(query);

        var item = new T();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            if (response.Count > 1)
                throw new InvalidDataException();

            item = response.FirstOrDefault();
        }
        return item;
    }
}