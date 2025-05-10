using PPDesk.Abstraction.DTO.Response.Eventbride.Pagination;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PPDesk.Abstraction.DTO.Response.Eventbride.Order
{
    public class EOrdersResponse : EPaginationResponse
    {
        [JsonPropertyName("orders")]
        public IEnumerable<EOrderResponse> Orders { get; set; }
    }
}
