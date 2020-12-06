using NBitcoin;
using NBitcoin.Protocol;

namespace CrossChained.BTP.NBitcoinSV
{
    public class BSVConsensusFactory: ConsensusFactory
    {
        private BSVConsensusFactory() { }
        public static BSVConsensusFactory Instance { get; } = new BSVConsensusFactory();

        public override ProtocolCapabilities GetProtocolCapabilities(uint protocolVersion)
        {
            var capabilities = base.GetProtocolCapabilities(protocolVersion);
            capabilities.SupportWitness = false;
            return capabilities;
        }

        public override Transaction CreateTransaction() =>
            new NBitcoin.Altcoins.ForkIdTransaction(0x00, false, this);

        public TransactionBuilder CreateTransactionBuilder(Network network) =>
            network.CreateTransactionBuilder().ContinueToBuild(CreateTransaction());
    }
}