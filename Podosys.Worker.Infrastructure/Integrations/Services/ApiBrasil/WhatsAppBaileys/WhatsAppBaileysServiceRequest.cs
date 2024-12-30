using Newtonsoft.Json;
using System;

namespace Podosys.Worker.Infrastructure.Integrations.Services.ApiBrasil.WhatsAppBaileys
{
    public class WhatsAppBaileysServiceRequest
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("options")]
        public Options Options { get; set; } = new Options();

        [JsonProperty("textMessage")]
        public TextMessage TextMessage { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }
    }

    public class TextMessage
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Options
    {
        [JsonProperty("delay")]
        public int Delay { get; set; } = 1;

        [JsonProperty("presence")]
        public string Presence { get; set; } = "composing";
    }
}
