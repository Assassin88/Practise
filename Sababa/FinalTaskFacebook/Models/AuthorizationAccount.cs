using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using winsdkfb;

namespace FinalTaskFacebook.Models
{
    internal class AuthorizationAccount
    {
        /// <summary>
        /// Initializes the current account with authorized user data.
        /// </summary>
        /// <param name="param">Serialized user object.</param>
        /// <param name="session">Сurrent user session.</param>
        /// <returns></returns>
        internal static Task<Account> GetInitializeAccountAsync(object param, FBSession session)
        {
            return Task.Run(() =>
            {
                var account = new Account()
                {
                    Id = session.User.Id,
                    Name = session.User.Name,
                    UriPicture = session.User.Picture.Data.Url
                };

                var users = JObject.Parse(param.ToString());

                foreach (var item in users.GetValue("data"))
                {
                    account.AccountFriends.Add(new UserFriend()
                    {
                        Id = (string)item["id"],
                        Name = (string)item["name"],
                        UriPicture = (string)item["picture"]["data"]["url"]
                    });
                }

                return account;
            });
        }
    }
}