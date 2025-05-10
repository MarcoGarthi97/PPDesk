using System.Text.Json.Serialization;

namespace PPDesk.Abstraction.DTO.Response.Eventbride.Pagination
{
    public class EPagination
    {
        [JsonPropertyName("object_count")]
        public int MaxCount { get; set; }
        [JsonPropertyName("page_number")]
        public int PageNumber { get; set; }
        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }
        [JsonPropertyName("page_count")]
        public int PageCount { get; set; }
        [JsonPropertyName("has_more_items")]
        public bool HasMoreItems { get; set; }
    }
}
