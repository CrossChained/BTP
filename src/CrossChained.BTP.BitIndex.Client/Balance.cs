using Newtonsoft.Json;

namespace CrossChained.BTP.BitIndex.Client
{
    public class Balance
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("txid")]
        public string TxId { get; set; }

        [JsonProperty("vout")]
        public uint OutIndex { get; set; }

        [JsonProperty("value")]
        public decimal Value { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("confirmations")]
        public long Confirmations { get; set; }

        [JsonProperty("scriptPubKey")]
        public string ScriptPubKey { get; set; }

        [JsonProperty("satoshis")]
        public long Satoshis { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }
}