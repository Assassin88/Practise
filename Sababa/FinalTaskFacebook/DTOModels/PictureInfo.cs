using Newtonsoft.Json;

namespace FinalTaskFacebook.DTOModels
{
    public class PictureInfo
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
