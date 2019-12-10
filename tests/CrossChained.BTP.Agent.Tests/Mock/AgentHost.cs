using System;
using System.Collections.Generic;
using System.Text;

namespace CrossChained.BTP.Agent.Tests.Mock
{
    class AgentHost
    {
        private NBitcoin.Key agent_key_ = new NBitcoin.Key();
        private readonly CustomWebApplicationFactory<Startup> agent_;

        public CustomWebApplicationFactory<Startup> Agent => this.agent_;
        public NBitcoin.Key Key => this.agent_key_;

        public AgentHost(int port)
        {
            this.agent_ = new CustomWebApplicationFactory<Startup>(this.agent_key_, port);
        }
    }
}
