using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossChained.BTP.NBitcoinSV.Client.impl
{
    class BitcoinSVApi : IBitcoinSVApi
    {
        private readonly BitIndex.Client.IBitIndexApi bitIndexApi_;

        public BitcoinSVApi(
            BitIndex.Client.IBitIndexApi bitIndexApi)
        {
            this.bitIndexApi_ = bitIndexApi;
        }

        public async Task<NBitcoin.TransactionBuilder> PrepareTransaction(NBitcoin.Key from, string to, decimal amount)
        {
            var fromAddress = from.PubKey.GetAddress(NBitcoin.ScriptPubKeyType.Legacy, this.bitIndexApi_.Network).ToString();
            var balance = await this.bitIndexApi_.GetBalance(fromAddress);

            var unspentCoins = new List<NBitcoin.Coin>();
            decimal current_balance = 0M;
            if (balance.Length > 0)
            {
                foreach (var coin in balance.OrderBy(x => x.Amount))
                {
                    if (0 < coin.Amount)
                    {
                        unspentCoins.Add(new NBitcoin.Coin(
                            NBitcoin.uint256.Parse(coin.TxId),
                            coin.OutIndex,
                            NBitcoin.Money.Coins(coin.Amount),
                            from.ScriptPubKey));

                        current_balance += coin.Amount;
                        if(current_balance >= amount)
                        {
                            break;
                        }
                    }
                }
            }

            if (current_balance < amount)
            {
                throw new Exception("Not enough money");
            }

            var builder = BSVConsensusFactory.Instance.CreateTransactionBuilder();
            builder.AddKeys(from);
            builder.AddCoins(unspentCoins);
            builder.Send(NBitcoin.BitcoinAddress.Create(to, this.bitIndexApi_.Network), NBitcoin.Money.Coins(amount));
            builder.SetChange(from.ScriptPubKey);

            return builder;
        }

        public decimal EstimateFee(NBitcoin.Transaction transaction)
        {
            return 0.00000001m * (transaction.ToHex().Length / 2 + 4);
        }

    }
}
