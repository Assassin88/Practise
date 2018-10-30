using System;
using System.Threading.Tasks;
using FacebookClient.Exceptions;
using FacebookClient.Models;
using FacebookClient.Services.Abstraction;
using Windows.Security.Authentication.Web;
using winsdkfb;

namespace FinalTaskFacebook.Services.Implementation
{
    public class AuthorizationSocialNetwork : ISocialNetwork
    {
        private readonly FBSession _session;

        public AuthorizationSocialNetwork()
        {
            _session = FBSession.ActiveSession;
            _session.WinAppId = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();
        }

        public async Task<Account> AuthorizeAsync(string userId, params string[] permissions)
        {
            _session.FBAppId = userId;
            var fbResult = await _session.LoginAsync(new FBPermissions(permissions));

            if (!fbResult.Succeeded)
                throw new LoginSessionException("The login on Facebook had invalid result. Please, log in Facebook!!!");

            return GetAccount();
        }

        public async Task LogoutAsync()
        {
            await _session.LogoutAsync();
        }

        public string Token => _session.AccessTokenData.AccessToken;

        private Account GetAccount()
        {
            return new Account()
            {
                Id = _session.User.Id,
                Name = _session.User.Name,
                UriPicture = _session.User.Picture.Data.Url
            };
        }

    }
}