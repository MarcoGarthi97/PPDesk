using System.Text.Json.Serialization;

namespace PPDesk.Abstraction.DTO.Response.Eventbride.Order
{
    public class EProfileResponse
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("cell_phone")]
        public string CellPhone { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
