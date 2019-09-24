using System;
using Xunit;

namespace CrossChained.BTP.NBitcoinSV.Tests
{
    public class BProtocolTests
    {
        private const string SampleScriptHex = "6a2231394878696756345179427633744870515663554551797131707a5a56646f4175744c8a7b222474797065223a22426574222c22757365725f6964223a2239383138222c226d61726b65745f6964223a392c2273696465223a22627579222c22616d6f756e74223a22302e3031222c22656e7472795f7072696365223a22222c226c65766572616765223a32302c2273746f705f6c6f7373223a22222c2274616b655f70726f666974223a22227d046a736f6e086265742e6a736f6e";
        private const string SampleFileBody = "{\"$type\":\"Bet\",\"user_id\":\"9818\",\"market_id\":9,\"side\":\"buy\",\"amount\":\"0.01\",\"entry_price\":\"\",\"leverage\":20,\"stop_loss\":\"\",\"take_profit\":\"\"}";
        private const string SampleFileMimeType = "json";
        private const string SampleFileName = "bet.json";

        [Fact]
        public void FileScriptTest()
        {
            var f = BProtocol.ParseScript(NBitcoin.Script.FromHex(SampleScriptHex));
            Assert.NotNull(f);
            Assert.Equal(SampleFileBody, System.Text.Encoding.UTF8.GetString(f.Body));
            Assert.Equal(SampleFileMimeType, f.MimeType);
            Assert.Equal(SampleFileName, f.Name);
        }

        [Fact]
        public void ScriptTest()
        {
            var f = new BProtocol.FileScript
            {
                Body = System.Text.Encoding.UTF8.GetBytes(SampleFileBody),
                MimeType = SampleFileMimeType,
                Name = SampleFileName
            };

            Assert.Equal(SampleScriptHex, BProtocol.GetScript(f).ToHex());
        }
    }
}
