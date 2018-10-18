using Newtonsoft.Json;

namespace FacebookClient.DTOModels
{
    public class PictureFb
    {
        [JsonProperty("data")]
        public PictureInfoFb PictureInfoFb { get; set; }
    }
}
