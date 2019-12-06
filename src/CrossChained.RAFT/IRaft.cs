using System;
using System.Threading.Tasks;

namespace CrossChained.RAFT
{
    public interface IRaft
    {
        Task Send(string message);
    }
}
