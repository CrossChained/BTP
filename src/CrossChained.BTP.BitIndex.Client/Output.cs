using Newtonsoft.Json;

namespace CrossChained.BTP.BitIndex.Client
{
    public class Output
    {
        [JsonProperty("value")]
        public decimal Value { get; set; }

        [JsonProperty("valueSat")]
        public long ValueSatoshis { get; set; }

        [JsonProperty("n")]
        public uint N { get; set; }

        [JsonProperty("scriptPubKey")]
        public ScriptPubKey ScriptPubKey { get; set; }

        [JsonProperty("spentTxId")]
        public string SpentTxId { get; set; }

        [JsonProperty("spentIndex")]
        public int? SpentIndex { get; set; }

        [JsonProperty("spentHeight")]
        public long? SpentHeight { get; set; }
    }
}