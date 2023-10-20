using System;
using Newtonsoft.Json;

namespace Howest.Mct.Functions.CosmosDb.Tasks.Models;

public class User
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("firstName")]
    public string FirstName { get; set; }
    [JsonProperty("email")]
    public string Email { get; set; }
}