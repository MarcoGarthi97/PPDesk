using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PPDesk.Abstraction.DTO.Response.Eventbride
{
    public class ETokenResponse
    {
        [JsonPropertyName("access_token")]
        public string Token { get; set; }
        [JsonPropertyName("token_type")]
        public string Type { get; set; }
    }
}
