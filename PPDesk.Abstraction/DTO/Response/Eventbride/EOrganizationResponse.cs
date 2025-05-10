using System;
using System.Text.Json.Serialization;

namespace PPDesk.Abstraction.DTO.Response.Eventbride
{
    public class EOrganizationResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("created")]
        public DateTime Created { get; set; } 
    }
}
