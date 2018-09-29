using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FinalTaskFacebook.Models
{
    public class FacebookAgent : IFacebookAgent
    {
        private readonly HttpClient _httpClient;

        public FacebookAgent()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://graph.facebook.com/v3.1/") };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> GetRemoteClientAsync<T>(string accessToken, string endpoint, string args = null)
        {
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
            if (!response.IsSuccessStatusCode)
                return default(T);

            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
    }
}
