using System;
using System.Threading.Tasks;

namespace CrossChained.BTP.BitIndex.Client
{
    public interface IBitIndexApi
    {
        Task<Balance[]> GetBalance(string address);
        Task<AddressInfo> GetAddressInfo(string address);
        Task<Transaction> GetTransaction(string trxid);
        Task Monitore(string address, string webhookUrl, string secret);
        Task Broadcast(string transactionBody);
        NBitcoin.Network Network { get; }
    }
}
