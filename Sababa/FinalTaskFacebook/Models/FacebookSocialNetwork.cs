using System;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using winsdkfb;

namespace FinalTaskFacebook.Models
{
    public class FacebookSocialNetwork : IFacebookSocialNetwork
    {
        private readonly string _userId;
        private readonly string[] _permissions;
        private readonly FBSession _session = FBSession.ActiveSession;
        private readonly IFacebookAgent _facebookAgent;

        public FacebookSocialNetwork(IFacebookAgent facebook, string userId = null, string[] permissions = null)
        {
            _facebookAgent = facebook;
            _userId = userId ?? "936346953231113";
            _permissions = permissions ?? new string[] { "public_profile", "email", "user_friends" };
        }

        public async Task<FBResult> Authorize()
        {
            _session.FBAppId = _userId;
            _session.WinAppId = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();
            return await _session.LoginAsync(new FBPermissions(_permissions));
        }

        public async Task<Account> GetAccountAsync(FBResult fbResult)
        {
            if (fbResult.Succeeded)
            {
                var result = await _facebookAgent.GetRemoteClientAsync<object>(_session.AccessTokenData.AccessToken, "me/friends", "fields=id,name,picture{url}");
                return await AuthorizationAccount.GetInitializeAccountAsync(result, _session);
            }

            throw new ArgumentException("The argument typeOf FBResult has invalid value.");
        }

        public async Task ClearSession()
        {
            await _session.LogoutAsync();
        }

    }
}
