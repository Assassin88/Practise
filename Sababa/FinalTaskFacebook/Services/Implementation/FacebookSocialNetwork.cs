using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using AutoMapper;
using FinalTaskFacebook.DTOModels;
using FinalTaskFacebook.Services.Abstraction;
using winsdkfb;
using FinalTaskFacebook.Models;

namespace FinalTaskFacebook.Services.Implementation
{
    public class FacebookSocialNetwork : ISocialNetwork
    {
        private readonly HttpClient _httpClient;
        private readonly FBSession _session;

        public FacebookSocialNetwork()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://graph.facebook.com/v3.1/") };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _session = FBSession.ActiveSession;
            _session.WinAppId = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();
        }

        public async Task<FBResult> Authorize(string userId, params string[] permissions)
        {
            _session.FBAppId = userId;
            return await _session.LoginAsync(new FBPermissions(permissions));
        }

        public async Task<Account> GetAccountAsync(FBResult fbResult, string endpoint, string args = null)
        {
            if (!fbResult.Succeeded)
                throw new ArgumentException("The argument typeOf FBResult has invalid value.");

            return await InitializeAccount(endpoint, args);
        }

        private async Task<Account> InitializeAccount(string endpoint, string args)
        {
            Account account = new Account()
            {
                Id = _session.User.Id,
                Name = _session.User.Name,
                UriPicture = _session.User.Picture.Data.Url
            };

            var accountFriends = await GetRemoteClientAsync<UserFriendsFb>(_session.AccessTokenData.AccessToken, endpoint, args);
            account.AccountFriends = accountFriends.Friends.Select(fb => Mapper.Map<UserFriend>(fb)).ToList();
            return account;
        }

        private async Task<T> GetRemoteClientAsync<T>(string accessToken, string endpoint, string args = null)
        {
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}{args}");
            response.EnsureSuccessStatusCode();
            return await Serializer.DeserializeObject<T>(response);
        }

        public async Task ClearSession()
        {
            await _session.LogoutAsync();
        }

    }
}
