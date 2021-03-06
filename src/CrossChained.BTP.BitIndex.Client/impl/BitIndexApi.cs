﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NBitcoin;
using Newtonsoft.Json;

namespace CrossChained.BTP.BitIndex.Client.impl
{
    public class BitIndexApi : IBitIndexApi
    {
        private readonly ILogger<BitIndexApi> _logger;
        private readonly BitIndexApiConfig options_;
        private readonly HttpClient _client = new HttpClient();

        public Network Network
        {
            get
            {
                return Network.GetNetwork(this.options_.Network);
            }
        }

        public BitIndexApi(IOptions<BitIndexApiConfig> options,ILogger<BitIndexApi> logger)
        {
            _logger = logger;
            this.options_ = options.Value;
        }

        public async Task Broadcast(string transactionBody)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(this.options_.BaseUri), this.options_.Network.ToLower() + "/tx/send")))
            {
                request.Headers.Add("api_key", this.options_.ApiKey);
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(
                        new SendTxArguments { TransactionBody = transactionBody }),
                    Encoding.UTF8,
                    "application/json");

                var response = await _client.SendAsync(request);
                if (System.Net.HttpStatusCode.OK != response.StatusCode)
                {
                    await ThrowError("Unable to broadcast transaction.", response);
                }
            }
        }

        public Task<Balance[]> GetBalance(string address)
        {
            return GetAsync<Balance[]>($"/addr/{address}/utxo");
        }

        public Task<AddressInfo> GetAddressInfo(string address)
        {
            return GetAsync<AddressInfo>($"/addr/{address}");
        }

        public Task<Transaction> GetTransaction(string trxid)
        {
            return GetAsync<Transaction>($"/tx/{trxid}");
        }

        public async Task Monitore(string address, string webhookUrl, string secret)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Put, new Uri(new Uri(this.options_.BaseUri), this.options_.Network.ToLower() + "/webhook/endpoint")))
            {
                request.Headers.Add("api_key", this.options_.ApiKey);
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(new MonitorConfig
                    {
                        WebhookUrl = webhookUrl,
                        Enabled = true,
                        WebhookSecret = secret
                    }),
                    Encoding.UTF8, "application/json");
                var response = await _client.SendAsync(request);
                if(System.Net.HttpStatusCode.OK != response.StatusCode)
                {
                    await ThrowError("Unable to install web hook.", response);
                }
            }

            using (var request = new HttpRequestMessage(HttpMethod.Put, new Uri(new Uri(this.options_.BaseUri), this.options_.Network.ToLower() + "/webhook/monitored_addrs")))
            {
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
                var response = await _client.SendAsync(request);
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

        private async Task<T> GetAsync<T>(string url)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, new Uri(new Uri(this.options_.BaseUri), this.options_.Network.ToLower() + url)))
            {
                request.Headers.Add("api_key", this.options_.ApiKey);
                var response = await _client.SendAsync(request);
                var responseText = await response.Content.ReadAsStringAsync();
                try
                {
                    return JsonConvert.DeserializeObject<T>(responseText);
                }
                catch (Exception e)
                {
                    _logger.LogError("failed to parse data from bitindex: {ResponseText}", responseText);
                    throw;
                }
                
            }
            

        }
    }
}
