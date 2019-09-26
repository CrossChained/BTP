using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NBitcoin;
using Newtonsoft.Json;

namespace CrossChained.BTP.BitIndex.Client.impl
{
    class BitIndexApi : IBitIndexApi
    {
        private readonly BitIndexApiConfig options_;

        public Network Network
        {
            get
            {
                return Network.GetNetwork(this.options_.Network);
            }
        }

        public BitIndexApi(IOptions<BitIndexApiConfig> options)
        {
            this.options_ = options.Value;
        }

        public async Task Broadcast(string transactionBody)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(this.options_.BaseUri), this.options_.Network.ToLower() + "/tx/send"));
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(new SendTxArguments { TransactionBody = transactionBody }),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.SendAsync(request);
                if (System.Net.HttpStatusCode.OK != response.StatusCode)
                {
                    await ThrowError("Unable to broadcast transaction.", response);
                }
            }
        }

        public async Task<Balance[]> GetBalance(string address)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(new Uri(new Uri(this.options_.BaseUri), this.options_.Network.ToLower() + "/addr/" + address + "/utxo"));
                return JsonConvert.DeserializeObject<Balance[]>(response);
            }
        }

        public async Task<AddressInfo> GetAddressInfo(string address)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(new Uri(new Uri(this.options_.BaseUri), this.options_.Network.ToLower() + "/addr/" + address));
                return JsonConvert.DeserializeObject<AddressInfo>(response);
            }
        }

        public async Task<Transaction> GetTransaction(string trxid)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(new Uri(new Uri(this.options_.BaseUri), this.options_.Network.ToLower() + "/tx/" + trxid));
                return JsonConvert.DeserializeObject<Transaction>(response);
            }
        }

        public async Task Monitore(string address, string webhookUrl, string secret)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Put, new Uri(new Uri(this.options_.BaseUri), this.options_.Network.ToLower() + "/webhook/endpoint"));
                request.Headers.Add("api_key", this.options_.ApiKey);
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(new MonitorConfig
                    {
                        WebhookUrl = webhookUrl,
                        Enabled = true,
                        WebhookSecret = secret
                    }),
                    Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);
                if(System.Net.HttpStatusCode.OK != response.StatusCode)
                {
                    await ThrowError("Unable to install web hook.", response);
                }

                request = new HttpRequestMessage(HttpMethod.Put, new Uri(new Uri(this.options_.BaseUri), this.options_.Network.ToLower() + "/webhook/monitored_addrs"));
                request.Headers.Add("api_key", this.options_.ApiKey);
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(new MonitorAddress[]
                    {
                        new MonitorAddress
                        {
                            Address = address
                        }
                    }),
                    Encoding.UTF8, "application/json");
                response = await client.SendAsync(request);
                if (System.Net.HttpStatusCode.OK != response.StatusCode)
                {
                    await ThrowError("Unable to install web hook.", response);
                }
            }
        }

        private static async Task ThrowError(string operation, HttpResponseMessage response)
        {
            string body = string.Empty;
            try
            {
                body = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            { }

            throw new Exception($"{operation} {response.StatusCode} {response.ReasonPhrase} {body}");
        }
    }
}
