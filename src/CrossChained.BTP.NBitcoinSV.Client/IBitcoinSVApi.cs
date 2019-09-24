using System;
using System.Threading.Tasks;

namespace CrossChained.BTP.NBitcoinSV
{
    public interface IBitcoinSVApi
    {
        Task<NBitcoin.TransactionBuilder> PrepareTransaction(NBitcoin.Key from, string to, decimal amount);

        decimal EstimateFee(NBitcoin.Transaction trx);
    }
}
