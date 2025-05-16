using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PPDesk.Abstraction.DTO.Response.Eventbride.TicketClass
{
    public class ETicketClassResponse
    {
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("event_id")]
        public string EventId { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("capacity")]
        public int Capacity { get; set; }

        [JsonPropertyName("quantity_sold")]
        public int QuantitySold { get; set; }

        [JsonPropertyName("sales_end")]
        public DateTime SalesEnd { get; set; }
    }

}
