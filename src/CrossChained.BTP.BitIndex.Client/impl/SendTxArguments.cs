using Newtonsoft.Json;

namespace CrossChained.BTP.BitIndex.Client.impl
{
    internal class SendTxArguments
    {
        [JsonProperty("rawtx")]
        public string TransactionBody { get; set; }
    }
}