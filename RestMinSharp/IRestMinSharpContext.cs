
using RestMinSharp.Operations;
using RestMinSharp.Results;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestMinSharp
{
    public interface IRestMinSharpContext
    {
		bool IdentReceivedJsonStrings { get; set; }

		void AddBearerToken(string token);
        Task<RequestResult<T>> DeleteAsync<T>(string url);
        Task<RequestResult<T>> GetAsync<T>(string url);
        Task<ERequestResult<T, E>> GetAsync<T, E>(string url);
        Task<MemoryStreamRequestResult> GetStreamAsync(string url);
        Task<RequestResult<T>> PatchAsync<T>(string url, List<PatchOperation> operations);
        Task<RequestResult<T>> PatchAsync<T>(string url, PatchOperation operations);
        Task<RequestResult<T>> PostAsync<T>(string url, object payload);
        Task<RequestResult<T>> PostAsync<T>(string url, string json);
        Task<ERequestResult<T, E>> PostAsync<T, E>(string url, object payload);
        Task<RequestResult<T>> PutAsync<T>(string url);
        Task<RequestResult<T>> PutAsync<T>(string url, object payload);
        Task<ERequestResult<T, E>> PutAsync<T, E>(string url, object payload);
		string SerializeObject(object obj);
		void SetBearerToken(string token);
        Task<RequestResult<T>> Upload<T>(string url, string name, byte[] bytes, Method method);
        Task<RequestResult<T>> Upload<T>(string url, string name, string filePath, Method method);
    }
}