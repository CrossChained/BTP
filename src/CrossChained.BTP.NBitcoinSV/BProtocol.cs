using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossChained.BTP.NBitcoinSV
{
    public class BProtocol
    {
        public class FileScript
        {
            public const string Address = "19HxigV4QyBv3tHpQVcUEQyq1pzZVdoAut";
            public byte[] Body { get; set; }
            public string MimeType { get; set; }
            public string Name { get; set; }
        }

        public static NBitcoin.Script GetScript(FileScript file)
        {
            var ops = new List<NBitcoin.Op>();

            ops.Add(NBitcoin.OpcodeType.OP_RETURN);
            ops.Add(NBitcoin.Op.GetPushOp(Encoding.UTF8.GetBytes(FileScript.Address)));
            ops.Add(NBitcoin.Op.GetPushOp(file.Body));
            ops.Add(NBitcoin.Op.GetPushOp(Encoding.UTF8.GetBytes(file.MimeType)));
            ops.Add(NBitcoin.Op.GetPushOp(Encoding.UTF8.GetBytes(file.Name)));

            return new NBitcoin.Script(ops);
        }

        public static FileScript ParseScript(NBitcoin.Script script)
        {
            var ops = script.ToOps().ToList();

            if (
                ops.Count < 5
                || ops[0].Code != NBitcoin.OpcodeType.OP_RETURN
                || !Encoding.UTF8.GetBytes(FileScript.Address).SequenceEqual(ops[1].PushData)
            )
            {
                return null;
            }

            return new FileScript
            {
                Body = ops[2].PushData,
                MimeType = Encoding.UTF8.GetString(ops[3].PushData),
                Name = Encoding.UTF8.GetString(ops[4].PushData)
            };
        }
    }
}