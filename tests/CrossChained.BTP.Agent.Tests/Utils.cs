using System;
using System.Collections.Generic;
using System.Text;

namespace CrossChained.BTP.Agent.Tests
{
    public static class Utils
    {
        public static string SendTransaction(
            NBitcoinSV.IBitcoinSVApi bsvApi,
            BitIndex.Client.IBitIndexApi bitIndexApi,
            NBitcoin.Key user_key,
            string target_address,
            decimal margin_sum,
            string message,
            string message_name)
        {
            var tr = bsvApi.PrepareTransaction(user_key, target_address, margin_sum).Result;
            tr.Send(
                    (new API.FileInfo
                    {
                        body = System.Text.Encoding.UTF8.GetBytes(message),
                        type = "text",
                        name = message_name
                    }).GetScript(),
                NBitcoin.Money.Zero);

            var trx = tr.BuildTransaction(true);
            var trx_id = trx.GetHash().ToString();
            bitIndexApi.Broadcast(trx.ToHex()).Wait();
            return trx_id;
        }
    }
}
