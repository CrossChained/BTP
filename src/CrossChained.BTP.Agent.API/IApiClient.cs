using System;
using System.Threading.Tasks;

namespace CrossChained.BTP.Agent.API
{
    public interface IApiClient : IDisposable
    {
        Task<string> get_margin_pool_address(); 
        Task pending_position(string trx, OrderSide side, decimal amount);
        Task reject_position(string trx);
        Task cancel_position(string trx);
        Task open_position(string trx, decimal open_price);
        Task close_position(string trx, decimal close_price, CloseReason reason, decimal decay);
        Task close_session(DateTime close_time);
    }
}
