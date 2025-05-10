using System;
using System.Text.Json.Serialization;

namespace PPDesk.Abstraction.DTO.Response.Eventbride.Event
{
    public class EEventDateResponse
    {
        [JsonPropertyName("local")]
        public DateTime Local { get; set; }
    }
}
