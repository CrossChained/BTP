using System;
using System.Collections.Generic;
using System.Text;

namespace CrossChained.BTP.Agent.Tests.Mock
{
    class AgentHost
    {
        private NBitcoin.Key agent_key_ = new NBitcoin.Key();
        private CustomWebApplicationFactory<Startup> agent_;

        public CustomWebApplicationFactory<Startup> Agent => this.agent_;
        public NBitcoin.Key Key => this.agent_key_;

        public AgentHost()
        {
        }

        internal void start(int port, string[] public_keys, BitIndex.Client.IBitIndexApi bitIndexApi)
        {
            this.agent_ = new CustomWebApplicationFactory<Startup>(this.agent_key_, port, public_keys, bitIndexApi);
        }
    }
}
