using System.Collections.Generic;
using Newtonsoft.Json;

namespace FinalTaskFacebook.DTOModels
{
    public class UserFriendsFb
    {
        [JsonProperty("data")]
        public List<UserFriendFb> Friends { get; set; }
    }
}
