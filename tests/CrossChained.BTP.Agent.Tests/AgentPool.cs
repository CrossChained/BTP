using CrossChained.BTP.Agent.API;
using CrossChained.BTP.Agent.API.WebClient;
using CrossChained.BTP.Agent.Tests.Mock;
using CrossChained.BTP.BitIndex.Client;
using CrossChained.BTP.NBitcoinSV;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CrossChained.BTP.Agent.Tests
{
    internal class AgentPool
    {
        private readonly List<AgentHost> agents_ = new List<AgentHost>();

        internal IApiClient get_client(int index)
        {
            return new ApiClient(this.agents_[index].Agent.CreateClient());
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

        internal async Task start(int count)
        {
            for (int i = 0; i < count; ++i)
            {
                this.create_agent();
            }

            for (int i = 0; i < count; ++i)
            {
                this.start_agent(i,
                    this.agents_.Select(x => 
                    x.Key.PubKey.GetAddress(NBitcoin.ScriptPubKeyType.Legacy, NBitcoin.Network.Main).ToString()).ToArray());
            }

            for (int i = 0; i < count; ++i)
            {
                using (var client = this.agents_[i].Agent.CreateClient())
                {
                    using (var unlockMessage = new HttpRequestMessage(HttpMethod.Post, "/api/Admin/Unlock"))
                    {
                        unlockMessage.Content = new StringContent(
                            JsonConvert.SerializeObject(
                            new Dto.UnlockModelDto
                            {
                                Key = this.agents_[i].Key.ToString(NBitcoin.Network.Main)

                            }),
                            System.Text.Encoding.UTF8, "application/json");
                        var unlockResult = await client.SendAsync(unlockMessage);
                        Assert.Equal(unlockResult.StatusCode, System.Net.HttpStatusCode.OK);
                    }
                }
            }
        }

        internal void create_agent()
        {
            var host = new AgentHost();
            this.agents_.Add(host);
        }

        internal void start_agent(int index, string[] public_keys)
        {
            this.agents_[index].start(5000 + index, public_keys);
        }

        internal decimal get_user_balance(BSVUser user)
        {
            throw new NotImplementedException();
        }
    }
}