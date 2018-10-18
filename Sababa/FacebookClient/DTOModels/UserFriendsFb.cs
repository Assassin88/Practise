using System.Collections.Generic;
using Newtonsoft.Json;

namespace FacebookClient.DTOModels
{
    public class UserFriendsFb
    {
        [JsonProperty("data")]
        public List<UserFriendFb> Friends { get; set; }
    }
}
