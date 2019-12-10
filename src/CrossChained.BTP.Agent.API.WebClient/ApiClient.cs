using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrossChained.BTP.Agent.API.WebClient
{
    public class ApiClient : IApiClient
    {
        private HttpClient http_client_;

        public ApiClient(HttpClient httpClient)
        {
            this.http_client_ = httpClient;
        }

        public Task cancel_position(string trx)
        {
            throw new NotImplementedException();
        }

        public Task close_position(string trx, decimal close_price, CloseReason reason, decimal decay)
        {
            throw new NotImplementedException();
        }

        public Task close_session(DateTime close_time)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            this.http_client_.Dispose();
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
