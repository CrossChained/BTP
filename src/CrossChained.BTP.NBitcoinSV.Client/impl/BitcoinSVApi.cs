using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrossChained.BTP.BitIndex.Client;
using NBitcoin;
using Transaction = NBitcoin.Transaction;

namespace CrossChained.BTP.NBitcoinSV.Client.impl
{
    class BitcoinSVApi : IBitcoinSVApi
    {
        private readonly IBitIndexApi _bitIndexApi;

        public BitcoinSVApi(IBitIndexApi bitIndexApi)
        {
            _bitIndexApi = bitIndexApi;
        }

        public async Task<TransactionBuilder> PrepareTransaction(Key from, string to, decimal amount, decimal fee)
        {
            var fromAddress = from.PubKey.GetAddress(ScriptPubKeyType.Legacy, _bitIndexApi.Network).ToString();
            var utxos = await _bitIndexApi.GetBalance(fromAddress);

            var unspentCoins = new List<Coin>();
            var currentBalance = 0M;
            foreach (var utxo in utxos.OrderBy(x => x.Amount).SkipWhile(t => t.Amount <= 0))
            {
                unspentCoins.Add(new Coin(
                    uint256.Parse(utxo.TxId),
                    utxo.OutIndex,
                    Money.Satoshis(utxo.Satoshis),
                    from.ScriptPubKey
                ));

                currentBalance += utxo.Amount;
                if(currentBalance >= amount + fee)
                    break;
            }

            if (currentBalance < amount)
                throw new Exception("Not enough money");

            var builder = _bitIndexApi.Network.CreateTransactionBuilder()
                .AddKeys(from)
                .AddCoins(unspentCoins)
                .Send(BitcoinAddress.Create(to, _bitIndexApi.Network), Money.Coins(amount))
                .SetChange(from.ScriptPubKey)
                .SendFees(Money.Coins(fee));

            return builder;
        }

        public decimal EstimateFee(Transaction transaction)
        {
            return 0.00000001m * (transaction.ToHex().Length / 2 + 4);
        }

    }
}
