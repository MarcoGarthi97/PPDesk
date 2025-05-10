using System.Text.Json.Serialization;

namespace PPDesk.Abstraction.DTO.Response.Eventbride.Event
{
    public class EEventStringResponse
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
