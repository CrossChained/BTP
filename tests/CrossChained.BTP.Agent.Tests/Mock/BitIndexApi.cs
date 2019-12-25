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

        public async Task Broadcast(string transactionBody)
        {
            var tx = NBitcoin.Transaction.Parse(transactionBody);
            decimal sum = 0;
            foreach(var input in tx.Inputs)
            {
                bool is_found = false;
                foreach(var user in this.Users)
                {
                    if (input.IsFrom(user.Value.Key.PubKey))
                    {
                        foreach(var balance in user.Value.Balances)
                        {
                            if(balance.TxId == input.PrevOut.Hash.ToString())
                            {
                                is_found = true;
                                sum += balance.Amount;
                                user.Value.Balance -= balance.Amount;
                                user.Value.Balances = user.Value.Balances.Where(x => x != balance).ToArray();
                                break;
                            }
                        }
                        if (!is_found)
                        {
                            throw new Exception("Invalid transaction");
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (!is_found)
                {
                    throw new Exception("Invalid transaction");
                }
            }
            foreach(var output in tx.Outputs)
            {
                bool is_found = false;
                foreach (var user in this.Users)
                {
                    if (output.IsTo(user.Value.Key.PubKey))
                    {
                        user.Value.Balances = user.Value.Balances.Append(new Balance
                        {
                            Amount = output.Value.ToDecimal(MoneyUnit.BTC),
                            TxId = tx.GetHash().ToString()
                        }).ToArray();
                        is_found = true;
                        user.Value.Balance += output.Value.ToDecimal(MoneyUnit.BTC);
                        break;
                    }
                }
                if (!is_found)
                {
                    throw new Exception("Invalid transaction");
                }
            }
        }

        public Task<AddressInfo> GetAddressInfo(string address)
        {
            throw new NotImplementedException();
        }

        public async Task<Balance[]> GetBalance(string address)
        {
            BSVUser user;
            if(!this.Users.TryGetValue(address, out user))
            {
               return null;
            }

            return user.Balances;

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
