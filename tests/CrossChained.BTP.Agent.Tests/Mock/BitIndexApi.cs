using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossChained.BTP.BitIndex.Client;
using NBitcoin;

namespace CrossChained.BTP.Agent.Tests.Mock
{
    public class BitIndexApi : BitIndex.Client.IBitIndexApi
    {
        private readonly Dictionary<string, BSVUser> users_ = new Dictionary<string, BSVUser>();

        public Network Network => Network.Main;

        public Dictionary<string, BSVUser> Users { get => this.users_; }

        public Task Broadcast(string transactionBody)
        {
            throw new NotImplementedException();
        }

        public Task<AddressInfo> GetAddressInfo(string address)
        {
            throw new NotImplementedException();
        }

        public Task<Balance[]> GetBalance(string address)
        {
            throw new NotImplementedException();
        }

        public Task<BitIndex.Client.Transaction> GetTransaction(string trxid)
        {
            throw new NotImplementedException();
        }

        public Task Monitore(string address, string webhookUrl, string secret)
        {
            throw new NotImplementedException();
        }
    }
}
