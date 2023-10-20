using System;
using Newtonsoft.Json;

namespace Howest.Mct.Functions.CosmosDb.Tasks.Models;

public class TaskMetadata
{
    [JsonConstructor]
    public TaskMetadata(Guid id, string type, string? name, string? locationName, string? tagName)
    {
        Id = id;
        Type = type;
        Name = name ?? locationName ?? tagName ?? string.Empty;
    }

    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
}