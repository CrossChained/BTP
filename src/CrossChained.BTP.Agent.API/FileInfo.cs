using System;
using System.Collections.Generic;
using System.Text;

namespace CrossChained.BTP.Agent.API
{
    public class FileInfo
    {
        public byte[] body { get; set; }
        public string type { get; set; }
        public string name { get; set; }


        public NBitcoin.Script GetScript()
        {
            var ops = new List<NBitcoin.Op>();

            ops.Add(NBitcoin.OpcodeType.OP_RETURN);
            ops.Add(NBitcoin.Op.GetPushOp(System.Text.Encoding.UTF8.GetBytes("19HxigV4QyBv3tHpQVcUEQyq1pzZVdoAut")));
            ops.Add(NBitcoin.Op.GetPushOp(this.body));
            ops.Add(NBitcoin.Op.GetPushOp(System.Text.Encoding.UTF8.GetBytes(this.type)));
            ops.Add(NBitcoin.Op.GetPushOp(System.Text.Encoding.UTF8.GetBytes(this.name)));

            return new NBitcoin.Script(ops);
        }
    }
}
