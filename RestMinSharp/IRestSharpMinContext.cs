
using RestMinSharp.Operations;
using RestSharp;
using RestSharpMin.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestMinSharp
{
    public interface IRestSharpMinContext
    {
        void AddBearerToken(string token);
        Task<RequestResult<T>> DeleteAsync<T>(string url);
        Task<RequestResult<T>> GetAsync<T>(string url, List<Parameter> parameters = null);
        Task<RequestResult<T>> PatchAsync<T>(string url, List<PatchOperation> operations);
        Task<RequestResult<T>> PatchAsync<T>(string url, PatchOperation operations);
        Task<RequestResult<T>> PostAsync<T>(string url, object payload);
        Task<RequestResult<T>> PutAsync<T>(string url);
        Task<RequestResult<T>> PutAsync<T>(string url, object payload);
    }
}