
using Newtonsoft.Json;
using RestMinSharp.Notifications;
using RestMinSharp.Operations;
using RestMinSharp.Results;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RestMinSharp
{
    public class RestMinSharpContext : IRestMinSharpContext
    {
        public bool IsAuthorized { get; internal set; }
        public bool ShowJsonContent { get; set; }
        private readonly RestClient _client;
        private bool _hasJwtToken = false;
        public string Token { get; internal set; }
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

        }
        public RestMinSharpContext(string baseUrl)
        {
            BaseUrl = baseUrl ?? throw new ArgumentNullException("BaseUrl");
            _client = new RestClient(this.BaseUrl);
        }

        public RestMinSharpContext(string baseUrl, bool showJsonContent) : this(baseUrl)
        {
            ShowJsonContent = showJsonContent;
        }

        public void AddBearerToken(string token)
        {
            _hasJwtToken = true;
            Token = token;
            _client.AddDefaultHeader("Authorization", "Bearer " + token);
        }

        public MemoryStreamRequestResult CreateMemoryStreamResult(RestResponse res)
        {
            var result = new MemoryStreamRequestResult();

            if (ShowJsonContent)
            {
                Console.WriteLine(res.Content);
            }

            if (res.IsSuccessful)
            {
                this.IsAuthorized = result.IsAuthorized = true;
                result.Stream = new MemoryStream(res.RawBytes);
            }
            else
            {
                if (res.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    if (ShowJsonContent)
                    {
                        Console.WriteLine(res.Content);
                    }

                    result.Notifications.Add(new Notification("InternalServerError", res.Content));
                }
                else if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    this.IsAuthorized = result.IsAuthorized = false;
                    if (ShowJsonContent)
                    {
                        Console.WriteLine("Is Unauthorized");
                    }

                    result.Notifications.Add(new Notification("Unauthorized", "Unauthorized"));
                }
                else
                {
                    if (ShowJsonContent)
                    {
                        Console.WriteLine("Has Notifications");
                    }

                    this.IsAuthorized = result.IsAuthorized = true;
                    result.Notifications = JsonConvert.DeserializeObject<List<Notification>>(res.Content);
                }
            }

            return result;
        }
        public RequestResult<T> CreateResult<T>(RestResponse res)
        {
            var result = new RequestResult<T>();

            if (ShowJsonContent)
            {
                Console.WriteLine(res.Content);
            }

            if (res.IsSuccessful)
            {
                this.IsAuthorized = result.IsAuthorized = true;
                result.Data = JsonConvert.DeserializeObject<T>(res.Content);
            }
            else
            {
                if (res.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    if (ShowJsonContent)
                    {
                        Console.WriteLine(res.Content);
                    }

                    result.Notifications.Add(new Notification("InternalServerError", res.Content));
                }
                else if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    this.IsAuthorized = result.IsAuthorized = false;
                    if (ShowJsonContent)
                    {
                        Console.WriteLine("Is Unauthorized");
                    }

                    result.Notifications.Add(new Notification("Unauthorized", "Unauthorized"));
                }
                else
                {
                    if (ShowJsonContent)
                    {
                        Console.WriteLine("Has Notifications");
                    }

                    this.IsAuthorized = result.IsAuthorized = true;
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
        public async Task<RequestResult<T>> PostAsync<T>(string url, string json)
        {
            var request = new RestRequest(url, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddStringBody(json, DataFormat.Json);
            var res = await _client.ExecuteAsync(request);
            return CreateResult<T>(res);
        }
        public async Task<RequestResult<T>> Upload<T>(string url, string name, string filePath, Method method)
        {
            var request = new RestRequest(url, method);
            request.AddFile(name, filePath, name);
            var res = await _client.ExecuteAsync(request);
            return CreateResult<T>(res);
        }

        public async Task<RequestResult<T>> Upload<T>(string url, string name, byte[] bytes, Method method)
        {
            var request = new RestRequest(url, method);
            request.AddFile(name, bytes, name);
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

        public async Task<MemoryStreamRequestResult> GetStreamAsync(string url)
        {
            var request = new RestRequest(url, Method.Get);
            var response = await _client.ExecuteAsync(request);
            return CreateMemoryStreamResult(response);
        }
    }
}
