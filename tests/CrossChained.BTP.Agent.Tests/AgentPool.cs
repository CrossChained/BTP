using CrossChained.BTP.Agent.API;
using CrossChained.BTP.Agent.Tests.Mock;
using CrossChained.BTP.BitIndex.Client;
using CrossChained.BTP.NBitcoinSV;
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

        internal IApiClient get_client()
        {
            throw new NotImplementedException();
        }

        public AgentPool()
        {
        }

        internal Mock.BSVUser create_user(decimal start_sum)
        {
            throw new NotImplementedException();
        }

        internal IBitIndexApi get_bitindex_api()
        {
            throw new NotImplementedException();
        }

        internal IBitcoinSVApi get_bitcoin_api()
        {
            throw new NotImplementedException();
        }

        internal void start(int count)
        {
            for(int i = 0; i < count; ++i)
            {
                this.start_agent();
            }
        }

        internal TestServer start_agent()
        {
            var host = new CustomWebApplicationFactory<Startup>();
            this.agents_.Add(host);
            return host.Server;            
        }

        internal decimal get_user_balance(BSVUser user)
        {
            throw new NotImplementedException();
        }
    }
}