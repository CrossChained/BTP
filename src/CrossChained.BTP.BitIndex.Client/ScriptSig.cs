using Newtonsoft.Json;

namespace CrossChained.BTP.BitIndex.Client
{
    public class ScriptSig
    {
        [JsonProperty("asm")]
        public string Asm { get; set; }

        [JsonProperty("hex")]
        public string Hex { get; set; }
    }
}