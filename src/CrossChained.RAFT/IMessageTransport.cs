using System;
using System.Collections.Generic;
using System.Text;

namespace CrossChained.RAFT
{
    public interface IMessageTransport
    {
        IMessageConnection Open(Uri destination);
    }
}
