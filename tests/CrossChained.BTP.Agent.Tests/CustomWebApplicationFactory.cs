using CrossChained.BTP.BitIndex.Client;
using CrossChained.BTP.NBitcoinSV;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NBitcoin;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrossChained.BTP.Agent.Tests
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly Key agent_key_;
        private readonly int port_;
        private readonly string[] public_keys_;
        private readonly IBitIndexApi bitIndexApi_;
        private readonly IBitcoinSVApi bitcoinSVApi_;

        public CustomWebApplicationFactory(
            NBitcoin.Key agent_key,
            int port,
            string[] public_keys,
            BitIndex.Client.IBitIndexApi bitIndexApi,
            NBitcoinSV.IBitcoinSVApi bitcoinSVApi)
        {
            this.agent_key_ = agent_key;
            this.port_ = port;
            this.public_keys_ = public_keys;
            this.bitIndexApi_ = bitIndexApi;
            this.bitcoinSVApi_ = bitcoinSVApi;

        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return base.CreateWebHostBuilder().UseUrls($"http://localhost:{this.port_}/");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("test");
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    //.AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddSingleton<BitIndex.Client.IBitIndexApi>(this.bitIndexApi_);
                services.AddSingleton<NBitcoinSV.IBitcoinSVApi>(this.bitcoinSVApi_);

                services.AddSingleton<IOptions<Config.AgentConfig>>(new OptionsWrapper<Config.AgentConfig>(
                    new Config.AgentConfig
                    {
                        Address = this.agent_key_.PubKey.GetAddress(ScriptPubKeyType.Legacy, Network.Main).ToString(),
                        PublicKeys = this.public_keys_
                    }));

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                var config = sp.GetService<IConfiguration>();
                config["APICFG_PUBLICURL"] = "http://localhost";

            });
        }
    }
}
