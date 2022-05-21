
using Newtonsoft.Json;
using RestMinSharp.Notifications;
using RestMinSharp.Operations;
using RestMinSharp.Results;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestMinSharp
{
    public class RestMinSharpContext : IRestMinSharpContext
    {
        private readonly RestClient _client;
        private bool _hasJwtToken = false;
        public bool LastIsAuthorized { get; internal set; }
        public bool HasJwtToken
        {
            get
            {
                return _hasJwtToken;
            }
        }
        private string BaseUrl { get; set; }
        public RestMinSharpContext()
        {
            LastIsAuthorized = false;
        }
        public RestMinSharpContext(string baseUrl)
        {
            BaseUrl = baseUrl ?? throw new ArgumentNullException("BaseUrl");
            _client = new RestClient(this.BaseUrl);
            LastIsAuthorized = false;
        }

        public void AddBearerToken(string token)
        {
            _hasJwtToken = true;
            _client.AddDefaultHeader("Authorization", "Bearer " + token);
        }

        public RequestResult<T> CreateResult<T>(RestResponse res)
        {
            var result = new RequestResult<T>();
            Console.WriteLine(res.Content);
            if (res.IsSuccessful)
            {
                LastIsAuthorized = true;
                result.Data = JsonConvert.DeserializeObject<T>(res.Content);
            }
            else
            {
                if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    result.IsUnauthorized = true;
                    Console.WriteLine("Is Unauthorized");
                    LastIsAuthorized = false;
                    result.Notifications.Add(new Notification("Unauthorized", "Unauthorized"));
                }
                else
                {
                    LastIsAuthorized = true;
                    Console.WriteLine("Has Notifications");
                    result.Notifications = JsonConvert.DeserializeObject<List<Notification>>(res.Content);
                }
            }

            return result;
        }

        public async Task<RequestResult<T>> GetAsync<T>(string url)
        {
            RestRequest request = new RestRequest(url, Method.Get);
            request.AddHeader("Content-Type", "application/json");
            var res = await _client.ExecuteAsync(request);
            return CreateResult<T>(res);
        }

        public async Task<RequestResult<T>> DeleteAsync<T>(string url)
        {
            var request = new RestRequest(url, Method.Delete);
            request.AddHeader("Content-Type", "application/json");
            var res = await _client.ExecuteAsync(request);
            return CreateResult<T>(res);
        }
        public async Task<RequestResult<T>> PutAsync<T>(string url)
        {
            var request = new RestRequest(url, Method.Put);
            request.AddHeader("Content-Type", "application/json");
            var res = await _client.ExecuteAsync(request);
            return CreateResult<T>(res);
        }
        public async Task<RequestResult<T>> PutAsync<T>(string url, object payload)
        {
            var request = new RestRequest(url, Method.Put);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(payload);
            var res = await _client.ExecuteAsync(request);
            return CreateResult<T>(res);
        }
        public async Task<RequestResult<T>> PostAsync<T>(string url, object payload)
        {
            var request = new RestRequest(url, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(payload);
            var res = await _client.ExecuteAsync(request);
            return CreateResult<T>(res);
        }

        public async Task<RequestResult<T>> PatchAsync<T>(string url, List<PatchOperation> operations)
        {
            var request = new RestRequest(url, Method.Patch);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(operations);
            var res = await _client.ExecuteAsync(request);
            return CreateResult<T>(res);
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
