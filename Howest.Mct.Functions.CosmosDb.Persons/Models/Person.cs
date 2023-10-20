using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Howest.Mct.Functions.CosmosDb.Persons.Models;

public class Person
{
    [JsonProperty("firstName")]
    public string FirstName { get; set; }

    [JsonProperty("lastName")]
    public string LastName { get; set; }

    [JsonProperty("eMail")]
    public string EMail { get; set; }

    [JsonProperty("age")]
    public int Age { get; set; }

    [JsonProperty("locations")]
    public List<Location> Locations { get; set; }

    [JsonProperty("id")]
    public Guid Id { get; set; }
}

