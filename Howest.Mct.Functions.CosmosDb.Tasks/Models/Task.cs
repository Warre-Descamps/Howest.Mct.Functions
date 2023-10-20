using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using Newtonsoft.Json;

namespace Howest.Mct.Functions.CosmosDb.Tasks.Models;

public class Task
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("userId")]
    public Guid UserId { get; set; }
    [JsonProperty("title")]
    public string Title { get; set; }
    [JsonProperty("duration")]
    public string Duration { get; set; }
    [JsonProperty("cost")]
    public decimal Cost { get; set; }
    public List<TaskMetadata> Locations { get; set; }
    public List<TaskMetadata> Tags { get; set; }
}