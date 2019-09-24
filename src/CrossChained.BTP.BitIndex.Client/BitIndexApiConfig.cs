namespace CrossChained.BTP.BitIndex.Client
{
    public class BitIndexApiConfig
    {
        public string BaseUri { get; set; } = "https://api.bitindex.network/api/v3/";
        public string ApiKey { get; set; }
        public string Network { get; set; } = "Main";
    }
}