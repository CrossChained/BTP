using Newtonsoft.Json;

namespace CrossChained.BTP.BitIndex.Client.impl
{
    internal class MonitorAddress
    {
        [JsonProperty("addr")]
        public string Address { get; set; }
    }
}