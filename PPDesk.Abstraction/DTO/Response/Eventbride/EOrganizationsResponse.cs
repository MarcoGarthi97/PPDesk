using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PPDesk.Abstraction.DTO.Response.Eventbride
{
    public class EOrganizationsResponse
    {
        [JsonPropertyName("organizations")]
        public IEnumerable<EOrganizationResponse> Organizations { get; set; }
    }
}
