using System;
using System.Threading.Tasks;

namespace CrossChained.BTP.Agent.API.WebClient
{
    public class ApiClient : IApiClient
    {
        public Task cancel_position(string trx)
        {
            throw new NotImplementedException();
        }

        public Task close_position(string trx, decimal close_price, CloseReason reason, decimal decay)
        {
            throw new NotImplementedException();
        }

        public Task close_session(string trx, DateTime close_time)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<string> get_margin_pool_address()
        {
            throw new NotImplementedException();
        }

        public Task open_position(string trx, decimal open_price)
        {
            throw new NotImplementedException();
        }

        public Task pending_position(string trx, OrderSide side, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task reject_position(string trx)
        {
            throw new NotImplementedException();
        }
    }
}
