using System;
using System.Collections;
using System.Text.Json.Serialization;

namespace PPDesk.Abstraction.DTO.Response.Eventbride.Event
{
    public class EEventResponse
    {
        [JsonPropertyName("name")]
        public EEventNameResponse Name { get; set; }
        [JsonPropertyName("description")]
        public EEventDescriptionResponse Description { get; set; }
        [JsonPropertyName("start")]
        public EEventDateStartResponse Start { get; set; }
        [JsonPropertyName("end")]
        public EEventDateEndResponse End { get; set; }
        [JsonPropertyName("organization_id")]
        public string OrganizationId { get; set; }
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
