using Newtonsoft.Json;

namespace Howest.Mct.Functions.CosmosDb.Persons.Models;

public class Location
{
    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("street")]
    public string Street { get; set; }
}