using CrossChained.BTP.Agent.API;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CrossChained.BTP.Agent.Tests
{
    internal class AgentPool
    {
        private readonly List<CustomWebApplicationFactory<Startup>> agents_ = new List<CustomWebApplicationFactory<Startup>>();
        public AgentPool()
        {
        }

        internal void start(int count)
        {
            for(int i = 0; i < count; ++i)
            {
                this.start_agent();
            }
        }

        internal IApiClient ConnectApi(int i)
        {
            var client = this.agents_[i].Server.CreateWebSocketClient();
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60)))
            {
                var ws = client.ConnectAsync(new Uri("ws://localhost:8080"), cts.Token).Result;
                return new WSClient.ApiClient(ws);
            }
        }

        internal TestServer start_agent()
        {
            var host = new CustomWebApplicationFactory<Startup>();
            this.agents_.Add(host);
            return host.Server;            
        }
    }
}