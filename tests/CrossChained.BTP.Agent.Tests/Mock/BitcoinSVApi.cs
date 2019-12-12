using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NBitcoin;

namespace CrossChained.BTP.Agent.Tests.Mock
{
    public class BitcoinSVApi : NBitcoinSV.IBitcoinSVApi
    {
        public decimal EstimateFee(Transaction trx)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionBuilder> PrepareTransaction(Key from, string to, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
