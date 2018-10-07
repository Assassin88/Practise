using Newtonsoft.Json.Linq;
using winsdkfb;

namespace FinalTaskFacebook.Models
{
    internal class AccountInitializer
    {
        /// <summary>
        /// Get the current account with authorized user data.
        /// </summary>
        /// <jsonFriends name="jsonFriends">Serialized user object.</jsonFriends>
        /// <jsonFriends name="session">Сurrent user session.</jsonFriends>
        /// <returns></returns>
        internal static Account GetAccount(object jsonFriends, FBSession session)
        {
            return InitializeAccount(jsonFriends, session);
        }

        private static Account InitializeAccount(object jsonFriends, FBSession session)
        {
            var account = new Account()
            {
                Id = session.User.Id,
                Name = session.User.Name,
                UriPicture = session.User.Picture.Data.Url
            };

            var users = JObject.Parse(jsonFriends.ToString());
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
        }
    }
}