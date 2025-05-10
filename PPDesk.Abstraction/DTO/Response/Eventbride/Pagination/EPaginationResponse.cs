using System.Text.Json.Serialization;

namespace PPDesk.Abstraction.DTO.Response.Eventbride.Pagination
{
    public class EPaginationResponse
    {
        [JsonPropertyName("pagination")]
        public EPagination Pagination { get; set; }
    }
}
