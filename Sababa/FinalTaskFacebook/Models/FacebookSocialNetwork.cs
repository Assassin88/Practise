using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using winsdkfb;

namespace FinalTaskFacebook.Models
{
    public class FacebookSocialNetwork : ISocialNetwork
    {
        private readonly string _userId = "936346953231113";
        private readonly string[] _permissions = { "public_profile", "email", "user_friends" };
        private readonly HttpClient _httpClient;
        private readonly FBSession _session = FBSession.ActiveSession;

        public FacebookSocialNetwork()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://graph.facebook.com/v3.1/") };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<FBResult> Authorize()
        {
            _session.FBAppId = _userId;
            _session.WinAppId = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();
            return await _session.LoginAsync(new FBPermissions(_permissions));
        }

        public async Task<Account> GetAccountAsync(FBResult fbResult, string endpoint, string args = null)
        {
            if (!fbResult.Succeeded)
                throw new ArgumentException("The argument typeOf FBResult has invalid value.");

            var result = await GetRemoteClientAsync<object>(_session.AccessTokenData.AccessToken, endpoint, args);
            return AccountInitializer.GetAccount(result, _session);
        }

        private async Task<T> GetRemoteClientAsync<T>(string accessToken, string endpoint, string args = null)
        {
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}{args}");
            if (!response.IsSuccessStatusCode)
                return default(T);

            return await Serializer.DeserializeObject<T>(response);
        }

        public async Task ClearSession()
        {
            await _session.LogoutAsync();
        }

    }
}
