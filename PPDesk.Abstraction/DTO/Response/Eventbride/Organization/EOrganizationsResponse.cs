using PPDesk.Abstraction.DTO.Response.Eventbride.Pagination;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PPDesk.Abstraction.DTO.Response.Eventbride.Organization
{
    public class EOrganizationsResponse : EPaginationResponse
    {
        [JsonPropertyName("organizations")]
        public IEnumerable<EOrganizationResponse> Organizations { get; set; }
    }
}
