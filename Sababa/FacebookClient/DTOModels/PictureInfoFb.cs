using Newtonsoft.Json;

namespace FacebookClient.DTOModels
{
    public class PictureInfoFb
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
