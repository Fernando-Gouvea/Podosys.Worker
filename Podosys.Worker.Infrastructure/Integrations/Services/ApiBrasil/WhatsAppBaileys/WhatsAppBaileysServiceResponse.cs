using Newtonsoft.Json;

namespace Podosys.Worker.Infrastructure.Integrations.Services.ApiBrasil.WhatsAppBaileys
{
    public class WhatsAppBaileysServiceResponse
    {
        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("response")]
        public Response Response { get; set; }
    }

    public class Response
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}