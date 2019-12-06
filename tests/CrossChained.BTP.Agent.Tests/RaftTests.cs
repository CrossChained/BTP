using System;
using Xunit;

namespace CrossChained.BTP.Agent.Tests
{
    public class RaftTests
    {
        [Fact]
        public void LeaderTest()
        {
            const int agent_count = 20;
            const decimal user1_start = 10m;
            const decimal user1_bit1 = 1m;

            var pool = new AgentPool();
            pool.start(agent_count);

            //var client = this.GetClient();
            //var user1 = this.CreateUser(user1_start);
            //this.Send(
            //    client.CurrentAddress("margin"),
            //    user1.Address,
            //    user1_bit1,
            //    (new FileInfo
            //    {
            //        body = System.Text.Encoding.UTF8.GetBytes(
            //            position.closed_price.ToString().TrimEnd('0')
            //            ),
            //        type = "text",
            //        name = "B"
            //    }).GetScript()
            //);
            string leader = null;
            for(int i = 0; i < agent_count; ++i)
            {
                using(var api = pool.ConnectApi(i))
                {
                    //var item = api.GetLeader();
                    //if(leader == null)
                    //{
                    //    leader = item;
                    //}
                    //else
                    //{
                    //    Assert.True(leader == item);
                    //}
                }
            }
        }
    }
}
