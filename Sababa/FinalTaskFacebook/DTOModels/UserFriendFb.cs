using Newtonsoft.Json;

namespace FinalTaskFacebook.DTOModels
{
    public class UserFriendFb
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("picture")]
        public PictureFb UriPicture { get; set; }

        [JsonProperty("music")]
        public MusicFb MusicCollection { get; set; }
    }
}
