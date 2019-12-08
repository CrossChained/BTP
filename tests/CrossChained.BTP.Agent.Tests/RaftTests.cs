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
            const decimal user1_bit_margin1 = 1m;
            const decimal user1_bit_leverage1 = 10;

            const decimal user2_start = 10m;
            const decimal user2_bit_margin1 = 1m;
            const decimal user2_bit_leverage1 = 10;

            const decimal entry_price1 = 10;
            const decimal entry_price2 = 10;

            const decimal close_price1 = 1;
            const decimal close_price2 = 20;

            var pool = new AgentPool();
            pool.start(agent_count);

            string margin_pool_address;
            using (var client = pool.get_client())
            {
                margin_pool_address = client.get_margin_pool_address().Result;
            }

            var user1 = pool.create_user(user1_start);
            var user2 = pool.create_user(user2_start);

            var bitIndexApi = pool.get_bitindex_api();
            var bsvApi = pool.get_bitcoin_api();

            var trx_id1 = Utils.SendTransaction(
                bsvApi,
                bitIndexApi,
                user1.Key,
                margin_pool_address,
                user1_bit_margin1,
                $"u1.1,m10,s0,a{user1_bit_leverage1 * user1_bit_margin1}",
                "B");

            var trx_id2 = Utils.SendTransaction(
                bsvApi,
                bitIndexApi,
                user2.Key,
                margin_pool_address,
                user2_bit_margin1,
                $"u1.2,m10,s0,a{user2_bit_leverage1 * user2_bit_margin1}",
                "B");

            for (int i = 0; i < agent_count; ++i)
            {
                using (var client = pool.get_client())
                {
                    client.pending_position(trx_id1, API.OrderSide.sell, user1_bit_leverage1 * user1_bit_margin1).Wait();

                    client.open_position(trx_id1, entry_price1).Wait();
                    client.open_position(trx_id2, entry_price2).Wait();

                    client.close_position(trx_id1, close_price1, API.CloseReason.manual, 0).Wait();
                    client.close_position(trx_id2, close_price2, API.CloseReason.manual, 0).Wait();

                    Assert.Equal(0, pool.get_user_balance(user1));
                    Assert.Equal(0, pool.get_user_balance(user2));

                    client.close_session(DateTime.UtcNow);
                }
            }
        }
    }
}
