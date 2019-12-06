using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace CrossChained.BTP.Agent.API
{
    public interface IWebSocketAPI
    {
        Task Start(WebSocket webSocket);
    }
}
