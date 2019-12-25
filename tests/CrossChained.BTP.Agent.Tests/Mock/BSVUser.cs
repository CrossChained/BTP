using System;
using System.Collections.Generic;
using System.Text;
using CrossChained.BTP.BitIndex.Client;

namespace CrossChained.BTP.Agent.Tests.Mock
{
    public class BSVUser
    {
        public NBitcoin.Key Key { get; set; }
        public decimal Balance { get; set; }
        public Balance[] Balances { get; set; }
    }
}
