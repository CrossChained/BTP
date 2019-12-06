using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrossChained.WebSockets
{
    public class WebSocketPipeline
    {
        private WebSocket websocket_;
        private SingleThreadApartment send_thread_ = new SingleThreadApartment();
        private SingleThreadApartment receive_thread_ = new SingleThreadApartment();
        private Thread work_thread_;
        private CancellationTokenSource is_shutting_down_ = new CancellationTokenSource();
        private int send_timeout_;
        private Func<string, Task> input_handler_;
        private Func<byte[], Task> binary_handler_;
        private Exception failed_ = null;
        private Action<Exception> error_handler_;

        public void start(
            WebSocket websocket,
            int send_timeout,
            Func<string, Task> input_handler,
            Action<Exception> error_handler,
            Func<byte[], Task> binary_handler = null)
        {
            this.websocket_ = websocket;
            this.send_timeout_ = send_timeout;
            this.input_handler_ = input_handler;
            this.binary_handler_ = binary_handler;
            this.error_handler_ = error_handler;

            this.work_thread_ = new Thread(receive_thread);
            this.work_thread_.Start();
        }

        public void stop()
        {
            this.is_shutting_down_.Cancel();

            this.send_thread_.join();
            this.receive_thread_.join();

            if (this.work_thread_ != null && this.work_thread_.IsAlive)
            {
                this.work_thread_.Join();
            }
        }
        public void enqueue(byte[] message)
        {
            if (null != this.failed_)
            {
                throw this.failed_;
            }

            try
            {
                this.send_thread_.invoke(() => send_thread(message));
            }
            catch (Exception exception)
            {
                this.set_error(exception);
                throw;
            }
        }

        private async Task send_thread(byte[] message)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(send_timeout_)))
            {
                await this.websocket_.SendAsync(new System.ArraySegment<byte>(message), WebSocketMessageType.Text, true, cts.Token);
            }
        }

        private void receive_thread()
        {
            try
            {
                var buffer = new System.IO.MemoryStream();
                while (!this.is_shutting_down_.IsCancellationRequested)
                {
                    var rcvBytes = new byte[1024];
                    var rcvBuffer = new ArraySegment<byte>(rcvBytes);

                    var rcvResult = this.websocket_.ReceiveAsync(
                        rcvBuffer,
                        this.is_shutting_down_.Token);

                    rcvResult.Wait();

                    buffer.Write(rcvBytes, rcvBuffer.Offset, rcvResult.Result.Count);

                    if (!rcvResult.Result.EndOfMessage)
                    {
                        continue;
                    }

                    switch (rcvResult.Result.MessageType)
                    {
                        case WebSocketMessageType.Text:
                        {
                            var body = Encoding.UTF8.GetString(buffer.ToArray());
                            buffer.SetLength(0);
                            this.receive_thread_.invoke(() => this.input_handler_(body));
                            break;
                        }
                        case WebSocketMessageType.Binary:
                            {
                                if(null == this.binary_handler_)
                                {
                                    throw new InvalidOperationException();
                                }

                                var body = buffer.ToArray();
                                buffer.SetLength(0);
                                this.receive_thread_.invoke(() => this.binary_handler_(body));

                                break;
                            }
                        case WebSocketMessageType.Close:
                            return;

                        default:
                            throw new InvalidOperationException();
                    }
                }
                return;
            }
            catch(Exception ex)
            {
                if (this.is_shutting_down_.IsCancellationRequested)
                {
                    return;
                }

                this.set_error(ex);
            }
        }

        private void set_error(Exception exception)
        {
            this.failed_ = exception;
            this.error_handler_(exception);
        }
    }
}