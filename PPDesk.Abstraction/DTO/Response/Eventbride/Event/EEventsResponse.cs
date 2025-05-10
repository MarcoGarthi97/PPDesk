using PPDesk.Abstraction.DTO.Response.Eventbride.Pagination;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PPDesk.Abstraction.DTO.Response.Eventbride.Event
{
    public class EEventsResponse : EPaginationResponse
    {
        [JsonPropertyName("events")]
        public IEnumerable<EEventResponse> Events { get; set; } 
    }
}
