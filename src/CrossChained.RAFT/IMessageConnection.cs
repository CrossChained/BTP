using System;
using System.Threading.Tasks;

namespace CrossChained.RAFT
{
    public interface IMessageConnection
    {
        Task Send(string message);

        void ErrorHandler(Func<Exception, Task> hadler);
        void MessageHandler(Func<string, Task> hadler);
        void CloseHandler(Action<Task> hadler);
    }
}