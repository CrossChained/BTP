using Newtonsoft.Json;

namespace CrossChained.BTP.BitIndex.Client.impl
{
    internal class MonitorConfig
    {
        [JsonProperty("url")]
        public string WebhookUrl { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("secret")]
        public string WebhookSecret { get; set; }
    }
}