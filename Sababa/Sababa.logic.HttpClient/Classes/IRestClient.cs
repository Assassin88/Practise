using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sababa.logic.HttpClient.Classes
{
    public interface IRestClient
    {
        Task<HttpResponseMessage> PostAsync<T>(T param, string uri, TypeOfConvert typeOfConvert, Dictionary<string, string> headers = null);
        Task<HttpResponseMessage> PutAsync<T>(T param, string uri, TypeOfConvert typeOfConvert, Dictionary<string, string> headers = null);
        Task<HttpResponseMessage> PatchAsync<T>(T param, string uri, TypeOfConvert typeOfConvert, Dictionary<string, string> headers = null);
        Task<HttpResponseMessage> DeleteAsync(string uri, Dictionary<string, string> headers = null);
        Task<HttpResponseMessage> GetAsync(string uri, Dictionary<string, string> headers = null);
        Task<T> GetValueAsync<T>(HttpResponseMessage message, TypeOfConvert typeOfConvert);
    }
}
