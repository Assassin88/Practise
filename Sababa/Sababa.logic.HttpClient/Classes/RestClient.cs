using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sababa.logic.HttpClient.Classes
{
    public class RestClient : IDisposable, IRestClient
    {
        private readonly System.Net.Http.HttpClient _httpClient;
        private static readonly HttpMethod _patch = new HttpMethod("PATCH");

        public RestClient(string baseAddress)
        {
            _httpClient = new System.Net.Http.HttpClient();
            _httpClient.BaseAddress = new Uri(baseAddress);
        }

        public async Task<HttpResponseMessage> PatchAsync<T>(T param, string uri, TypeOfConvert typeOfConvert, Dictionary<string, string> headers = null)
        {
            try
            {
                using (var httpRequestMessage = new HttpRequestMessage(_patch, $"{_httpClient.BaseAddress}{uri}"))
                {
                    httpRequestMessage.Content = new StreamContent(Serializer.Serialize(param, typeOfConvert));

                    if (headers != null)
                    {
                        AddHeaders(httpRequestMessage, headers);
                    }

                    return await _httpClient.SendAsync(httpRequestMessage);
                }
            }
            catch (ArgumentNullException)
            {
                //log
                throw;
            }
            catch (InvalidOperationException)
            {
                //log
                throw;
            }
            catch (HttpRequestException)
            {
                //log
                throw;
            }

        }

        public async Task<HttpResponseMessage> PostAsync<T>(T param, string uri, TypeOfConvert typeOfConvert, Dictionary<string, string> headers = null)
        {
            try
            {
                using (var streamContent = new StreamContent(Serializer.Serialize(param, typeOfConvert)))
                using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri))
                {
                    httpRequestMessage.Content = streamContent;

                    if (headers != null)
                    {
                        AddHeaders(httpRequestMessage, headers);
                    }

                    return await _httpClient.SendAsync(httpRequestMessage);
                };
            }
            catch (ArgumentNullException)
            {
                //log
                throw;
            }
            catch (InvalidOperationException)
            {
                //log
                throw;
            }
            catch (HttpRequestException)
            {
                //log
                throw;
            }

        }

        public async Task<HttpResponseMessage> PutAsync<T>(T param, string uri, TypeOfConvert typeOfConvert, Dictionary<string, string> headers = null)
        {
            try
            {
                using (var streamContent = new StreamContent(Serializer.Serialize(param, typeOfConvert)))
                using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, uri))
                {
                    httpRequestMessage.Content = streamContent;

                    if (headers != null)
                    {
                        AddHeaders(httpRequestMessage, headers);
                    }

                    return await _httpClient.SendAsync(httpRequestMessage);
                }
            }
            catch (ArgumentNullException)
            {
                //log
                throw;
            }
            catch (InvalidOperationException)
            {
                //log
                throw;
            }
            catch (HttpRequestException)
            {
                //log
                throw;
            }

        }

        public async Task<HttpResponseMessage> DeleteAsync(string uri, Dictionary<string, string> headers = null)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Delete, uri))
                {
                    if (headers != null)
                    {
                        AddHeaders(request, headers);
                    }

                    return await _httpClient.SendAsync(request);
                }
            }
            catch (ArgumentNullException)
            {
                //log
                throw;
            }
            catch (InvalidOperationException)
            {
                //log
                throw;
            }
            catch (HttpRequestException)
            {
                //log
                throw;
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string uri, Dictionary<string, string> headers = null)
        {
            using (var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                if (headers != null)
                {
                    AddHeaders(httpRequestMessage, headers);
                }

                return await _httpClient.SendAsync(httpRequestMessage);
            }
        }

        public async Task<T> GetValueAsync<T>(HttpResponseMessage message, TypeOfConvert typeOfConvert)
        {
            try
            {
                using (var responseParam = await message.Content.ReadAsStreamAsync())
                {
                    return Serializer.Deserialize<T>(responseParam, typeOfConvert);
                }
            }
            catch (ArgumentNullException)
            {
                //log
                throw;
            }
            catch (HttpRequestException)
            {
                //log
                throw;
            }
        }

        private void AddHeaders(HttpRequestMessage message, Dictionary<string, string> headers)
        {
            foreach (var item in headers)
            {
                message.Headers.Add(item.Key, item.Value);
            }
        }

        #region Dispose

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _httpClient.Dispose();
                }

                disposedValue = true;
            }
        }

        ~RestClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
