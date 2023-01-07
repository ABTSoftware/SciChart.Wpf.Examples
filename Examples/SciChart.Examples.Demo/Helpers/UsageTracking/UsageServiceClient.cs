﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using RestSharp;
using SciChart.Examples.Demo.Common;
using SciChart.UI.Bootstrap;
using SciChart.UI.Reactive;

namespace SciChart.Examples.Demo.Helpers.UsageTracking
{
    public interface IUsageServiceClient
    {
        Task<bool> SendLocalUsage(string userId, List<ExampleUsage> usage);

        Task<List<ExampleRating>> GetGlobalUsage();
    }

    [ExportType(typeof(IUsageServiceClient), CreateAs.Singleton)]
    public class UsageServiceClient : IUsageServiceClient
    {
        private readonly IUsageClientConfiguration _config;

        /// <param name="config">The configuration for the usage server</param>
        public UsageServiceClient(IUsageClientConfiguration config)
        {
            Validate.NotNull(config, "config");
            Validate.NotNull(config.Address, "config.Address");

            _config = config;
        }

        public Task<bool> SendLocalUsage(string userId, List<ExampleUsage> usage)
        {
            UsageData request = new UsageData { UserId = userId, Usage = usage };
            return Post(request, "Submit");
        }

        public Task<List<ExampleRating>> GetGlobalUsage()
        {
            var requestTask = Get<List<ExampleRating>>("Ratings");

            return requestTask;
        }

        internal class BoolResponse
        {
            public bool Result { get; set; }
        }

#if !SILVERLIGHT
        private Task<TResponse> Get<TResponse>(string action, IWebProxy proxy = null) where TResponse : new()
        {
            return Task.Factory.StartNew(() =>
            {
                var client = new RestClient(_config.Address);
                var getRequest = new RestRequest("api/usage/" + action, Method.GET);
                
                getRequest.AddHeader("Authorization", GetAuthHeader(null));

                client.Timeout = 30000;
                client.Proxy = proxy ?? GetProxyService();

                IRestResponse<TResponse> response = client.Execute<TResponse>(getRequest);

                if (response.StatusCode == 0 || response.StatusCode != HttpStatusCode.OK)
                {
                    return default(TResponse);
                }
                return response.Data;
            });
        }

        private Task<bool> Post<TRequest>(TRequest request, string action)
        {
            return Task.Factory.StartNew(() =>
            {
                var client = new RestClient(_config.Address);
                var restRequest = new RestRequest("api/usage/" + action, Method.POST) {RequestFormat = DataFormat.Json};

                restRequest.AddHeader("Authorization", GetAuthHeader(null));
                restRequest.AddJsonBody(request);

                client.Timeout = 30000;
                client.Proxy = GetProxyService();

                var response = client.Execute<BoolResponse>(restRequest);
                
                if (response.StatusCode == 0 || response.StatusCode != HttpStatusCode.OK)
                {
                    return false;
                }
                return response.Data?.Result ?? false;
            });
        }

        private IWebProxy GetProxyService()
        {
            return WebRequest.DefaultWebProxy;
        }

#else
        private Task<TResponse> Get<TResponse>(string action) where TResponse : new()
        {
            return Task.Factory.StartNew(() =>
            {
                var client = new RestClient(_config.Address);

                var getRequest = new RestRequest("api/usage/" + action, Method.GET);
                client.Timeout = 30000;
                TResponse result = default(TResponse);
                getRequest.AddHeader("Authorization", GetAuthHeader(null));
                client.ExecuteAsync<TResponse>(getRequest, response =>
                    {
                        if (response.StatusCode != 0 && response.StatusCode == HttpStatusCode.OK)
                        {
                            result = response.Data;
                        }
                    }
                );
                return result;
            });
        }

        private Task<bool> Post<TRequest>(TRequest request, string action)
        {
            return Task.Factory.StartNew(() =>
            {
                var client = new RestClient(_config.Address);

                var restRequest = new RestRequest("api/usage/" + action, Method.POST);
                restRequest.RequestFormat = DataFormat.Json;
                restRequest.AddBody(request);
                restRequest.AddHeader("Authorization", GetAuthHeader(null));
                client.Timeout = 30000;
                bool result = false;
                client.ExecuteAsync<BoolResponse>(restRequest, response =>
                    {
                        if (response != null && response.StatusCode != 0 && response.StatusCode == HttpStatusCode.OK)
                        {
                            result = response.Data.Result;
                        }
                    });
                return result;
            });
        }
#endif
        private const string secretKey = "MJhdbS7%FBuOAI64BAshfbjeGa";

        private string GetAuthHeader(string username)
        {
            // Get salt
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] four_bytes = new byte[4];
            rng.GetBytes(four_bytes);
            var salt = BitConverter.ToUInt32(four_bytes, 0).ToString();
            // Get timestamp
            string timestamp = DateTime.UtcNow.ToString("o");
            // create signature
            string rawSig = (username ?? "") + timestamp + salt;
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secretKey);
            byte[] messageBytes = encoding.GetBytes(rawSig);
            string signature;
            // hash and base64 encode 
            using (var sha256 = new HMACSHA256(keyByte))
            {
                byte[] hashMessage = sha256.ComputeHash(messageBytes);
                signature = Convert.ToBase64String(hashMessage);
            }
            // build auth header
            return timestamp + "," + salt + "," + signature;
        }
    }
}