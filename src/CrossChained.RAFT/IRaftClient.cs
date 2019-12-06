using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrossChained.RAFT
{
    public interface IRaftClient
    {
        Task StoreMessage(RaftMessage message);
        Task ApplyMessage(RaftMessageId msg_id);
    }
}
