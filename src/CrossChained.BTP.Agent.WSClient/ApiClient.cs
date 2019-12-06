using CrossChained.BTP.Agent.API;
using CrossChained.WebSockets;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace CrossChained.BTP.Agent.WSClient
{
    public class ApiClient : IApiClient
    {
        private WebSocketPipeline ws_;
        private Exception failed_;

        public ApiClient(WebSocket ws)
        {
            this.ws_.start(
                ws,
                60,
                x => this.input_handler(x),
                y => this.on_error(y));
        }

        private async Task input_handler(string message)
        {
            lock (this)
            {
            }
        }
        protected virtual void on_error(Exception ex)
        {
            lock (this)
            {
                this.ws_ = null;
                this.failed_ = ex;
            }
        }



        public void Dispose()
        {
            try
            {
                if (this.ws_ != null)
                {
                    this.ws_.stop();
                    this.ws_ = null;
                }
            }
            catch { }
        }
    }
}
