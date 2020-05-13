using System.ComponentModel.DataAnnotations;

namespace CrossChained.BTP.BitIndex.Client
{
    public class BitIndexApiConfig
    {
        [Required]
        public string BaseUri { get; set; } = "https://api.bitindex.network/api/v3/";

        [Required]
        public string ApiKey { get; set; }

        [Required]
        public string Network { get; set; } = "Main";
    }
}