using CrossChained.BTP.Agent.Tests.Mock;
using CrossChained.BTP.BitIndex.Client;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using Xunit;

namespace CrossChained.BTP.Agent.Tests
{
    public class BitIndexClientTests
    {
        private const string api_key = "8mEDL8GSykuX9YshoSRqppJKkbBAeTdQSfPMFMFui1iiF7vrUMB3nQhM376DQYiT1H";
        
        [Fact]
        public void When_GetBalance_Then_success()
        {
            var api = new CrossChained.BTP.BitIndex.Client.impl.BitIndexApi(new OptionsWrapper<BitIndexApiConfig>(new BitIndexApiConfig()
            {
                ApiKey = api_key
            }));
            var b = api.GetBalance("1CTKcxmjZF9fk8mgtHA4tCGpnC7CvWyRw").Result;
            Assert.NotNull(b);

        }
        
        [Fact]
        public void When_GetAddressInfo_Then_success()
        {
            var api = new CrossChained.BTP.BitIndex.Client.impl.BitIndexApi(new OptionsWrapper<BitIndexApiConfig>(new BitIndexApiConfig()
            {
                ApiKey = api_key
            }));
            var b = api.GetAddressInfo("1CTKcxmjZF9fk8mgtHA4tCGpnC7CvWyRw").Result;
            Assert.NotNull(b);

        }
        // [Fact]
        // public void When_GetAddressInfo_Then_success()
        // {
        //     var api = new CrossChained.BTP.BitIndex.Client.impl.BitIndexApi(new OptionsWrapper<BitIndexApiConfig>(new BitIndexApiConfig()
        //     {
        //         ApiKey = api_key
        //     }));
        //     var b = api.GetAddressInfo("1CTKcxmjZF9fk8mgtHA4tCGpnC7CvWyRw").Result;
        //     Assert.NotNull(b);
        //
        // }
    }
}