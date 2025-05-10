using PPDesk.Abstraction.DTO.Response.Eventbride.Pagination;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PPDesk.Abstraction.DTO.Response.Eventbride.TicketClass
{
    public class ETicketClassesResponse : EPaginationResponse
    {
        [JsonPropertyName("ticket_classes")]
        public IEnumerable<ETicketClassResponse> TicketClasses { get; set; }
    }

}
