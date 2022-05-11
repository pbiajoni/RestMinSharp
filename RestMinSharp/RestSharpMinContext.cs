
using Newtonsoft.Json;
using RestMinSharp.Notifications;
using RestMinSharp.Operations;
using RestSharp;
using RestSharpMin.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestMinSharp
{
    public class RestSharpMinContext : IRestSharpMinContext
    {
        private readonly RestClient _client;
        private string BaseUrl { get; set; }
        public RestSharpMinContext()
        {
        }
        public RestSharpMinContext(string baseUrl)
        {
            BaseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
            _client = new RestClient(this.BaseUrl);

        }

        public void AddBearerToken(string token)
        {
            _client.AddDefaultHeader("Authorization", "Bearer " + token);
        }

        public async Task<RequestResult<T>> GetAsync<T>(string url, List<Parameter> parameters = null)
        {
            RestRequest request = new RestRequest(url, Method.Get);
            request.AddHeader("Content-Type", "application/json");
            var res = await _client.ExecuteAsync(request);
            var result = new RequestResult<T>();

            if (res.IsSuccessful)
            {
                result.Data = JsonConvert.DeserializeObject<T>(res.Content);
            }
            else
            {
                result.Notifications = JsonConvert.DeserializeObject<List<Notification>>(res.Content);
            }

            return result;
        }

        public async Task<RequestResult<T>> DeleteAsync<T>(string url)
        {
            var request = new RestRequest(url, Method.Delete);
            request.AddHeader("Content-Type", "application/json");
            var res = await _client.ExecuteAsync(request);

            var result = new RequestResult<T>();

            if (res.IsSuccessful)
            {
                result.Data = JsonConvert.DeserializeObject<T>(res.Content);
            }
            else
            {
                result.Notifications = JsonConvert.DeserializeObject<List<Notification>>(res.Content);
            }

            return result;
        }
        public async Task<RequestResult<T>> PutAsync<T>(string url)
        {
            var request = new RestRequest(url, Method.Put);
            request.AddHeader("Content-Type", "application/json");
            var res = await _client.ExecuteAsync(request);

            var result = new RequestResult<T>();

            if (res.IsSuccessful)
            {
                result.Data = JsonConvert.DeserializeObject<T>(res.Content);
            }
            else
            {
                result.Notifications = JsonConvert.DeserializeObject<List<Notification>>(res.Content);
            }

            return result;
        }
        public async Task<RequestResult<T>> PutAsync<T>(string url, object payload)
        {
            var request = new RestRequest(url, Method.Put);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(payload);
            var res = await _client.ExecuteAsync(request);

            var result = new RequestResult<T>();

            if (res.IsSuccessful)
            {
                result.Data = JsonConvert.DeserializeObject<T>(res.Content);
            }
            else
            {
                result.Notifications = JsonConvert.DeserializeObject<List<Notification>>(res.Content);
            }

            return result;
        }
        public async Task<RequestResult<T>> PostAsync<T>(string url, object payload)
        {
            var request = new RestRequest(url, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(payload);
            var res = await _client.ExecuteAsync(request);
            var result = new RequestResult<T>();

            if (res.IsSuccessful)
            {
                result.Data = JsonConvert.DeserializeObject<T>(res.Content);
            }
            else
            {
                result.Notifications = JsonConvert.DeserializeObject<List<Notification>>(res.Content);
            }

            return result;
        }

        public async Task<RequestResult<T>> PatchAsync<T>(string url, List<PatchOperation> operations)
        {
            var request = new RestRequest(url, Method.Patch);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(operations);
            var res = await _client.ExecuteAsync(request);
            var result = new RequestResult<T>();

            if (res.IsSuccessful)
            {
                result.Data = JsonConvert.DeserializeObject<T>(res.Content);
            }
            else
            {
                result.Notifications = JsonConvert.DeserializeObject<List<Notification>>(res.Content);
            }

            return result;
        }

        public async Task<RequestResult<T>> PatchAsync<T>(string url, PatchOperation operations)
        {
            return await PatchAsync<T>(url, new List<PatchOperation>()
            {
                operations
            });
        }
    }
}
