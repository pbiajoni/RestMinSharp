using Newtonsoft.Json;
using RestMinSharp.Notifications;
using RestMinSharp.Operations;
using RestMinSharp.Results;
using RestMinSharp.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RestMinSharp
{
	public class RestMinSharpContext : IRestMinSharpContext
	{
		public delegate void OnContentReceivedEventHandler(string content);

		public event OnContentReceivedEventHandler OnContentReceived;

		public delegate void OnSentJsonObjectEventHandler(string jsonString);

		public event OnSentJsonObjectEventHandler OnSentJsonObject;

		/// <summary>
		/// Indent the JSON strings received in the OnContentReceived event.
		/// </summary>
		public bool IdentReceivedJsonStrings { get; set; } = true;

		public string Token { get; internal set; }

		public bool HasJwtToken
		{
			get
			{
				return _hasJwtToken;
			}
		}

		public bool IsAuthorized { get; internal set; }
		public bool ShowJsonContent { get; set; }
		private readonly RestClient _client;
		private bool _hasJwtToken = false;
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

		#region Methods

		[Obsolete("Use SetBearerToken instead")]
		public void AddBearerToken(string token)
		{
			_hasJwtToken = true;
			Token = token;
			_client.AddDefaultHeader("Authorization", "Bearer " + token);
		}

		public string SerializeObject(object obj)
		{
			return JsonConvert.SerializeObject(obj, Formatting.Indented);
		}

		public void SetBearerToken(string token)
		{
			_hasJwtToken = true;
			Token = token;

			if (_client.DefaultParameters != null && _client.DefaultParameters.Any(x => x.Name == "Authorization"))
			{
				var bearer = _client.DefaultParameters.FirstOrDefault(x => x.Name == "Authorization");
				_client.DefaultParameters.RemoveParameter(bearer);
			}

			_client.AddDefaultHeader("Authorization", "Bearer " + token);
		}

		public void InvokeOnContentReceived(string content)
		{
			if (OnContentReceived != null)
			{
				if (IdentReceivedJsonStrings)
				{
					OnContentReceived?.Invoke(JsonUtils.IdentJsonString(content));
				}
				else
				{
					OnContentReceived?.Invoke(content);
				}
			}
		}

		#endregion Methods

		#region CreateResults

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

			result.RawData = res.Content;
			InvokeOnContentReceived(res.Content);

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

		public ERequestResult<T, E> CreateEResult<T, E>(RestResponse res)
		{
			var result = new ERequestResult<T, E>();

			if (ShowJsonContent)
			{
				Console.WriteLine(res.Content);
			}

			result.RawData = res.Content;
			InvokeOnContentReceived(res.Content);

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
					result.Error = JsonConvert.DeserializeObject<E>(res.Content);
				}
			}

			return result;
		}

		#endregion CreateResults

		#region Requests

		public async Task<RequestResult<T>> GetAsync<T>(string url)
		{
			RestRequest request = new RestRequest(url, Method.Get);
			request.AddHeader("Content-Type", "application/json");
			var res = await _client.ExecuteAsync(request);
			return CreateResult<T>(res);
		}

		public async Task<ERequestResult<T, E>> GetAsync<T, E>(string url)
		{
			RestRequest request = new RestRequest(url, Method.Get);
			request.AddHeader("Content-Type", "application/json");
			var res = await _client.ExecuteAsync(request);
			return CreateEResult<T, E>(res);
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
			OnSentJsonObject?.Invoke(SerializeObject(payload));
			var request = new RestRequest(url, Method.Put);
			request.AddHeader("Content-Type", "application/json");
			request.AddJsonBody(payload);
			var res = await _client.ExecuteAsync(request);
			return CreateResult<T>(res);
		}

		/// <summary>
		///
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="E"></typeparam>
		/// <param name="url"></param>
		/// <param name="payload"></param>
		/// <returns></returns>
		public async Task<ERequestResult<T, E>> PutAsync<T, E>(string url, object payload)
		{
			OnSentJsonObject?.Invoke(SerializeObject(payload));
			var request = new RestRequest(url, Method.Put);
			request.AddHeader("Content-Type", "application/json");
			request.AddJsonBody(payload);
			var res = await _client.ExecuteAsync(request);
			return CreateEResult<T, E>(res);
		}

		public async Task<RequestResult<T>> PostAsync<T>(string url, object payload)
		{
			OnSentJsonObject?.Invoke(SerializeObject(payload));
			var request = new RestRequest(url, Method.Post);
			request.AddHeader("Content-Type", "application/json");
			request.AddJsonBody(payload);
			var res = await _client.ExecuteAsync(request);
			return CreateResult<T>(res);
		}

		public async Task<RequestResult<T>> PostAsync<T>(string url, string json)
		{
			OnSentJsonObject?.Invoke(json);
			var request = new RestRequest(url, Method.Post);
			request.AddHeader("Content-Type", "application/json");
			request.AddStringBody(json, DataFormat.Json);
			var res = await _client.ExecuteAsync(request);
			return CreateResult<T>(res);
		}

		public async Task<ERequestResult<T, E>> PostAsync<T, E>(string url, object payload)
		{
			OnSentJsonObject?.Invoke(SerializeObject(payload));
			var request = new RestRequest(url, Method.Post);
			request.AddHeader("Content-Type", "application/json");
			request.AddJsonBody(payload);
			var res = await _client.ExecuteAsync(request);
			return CreateEResult<T, E>(res);
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

		#endregion Requests
	}
}