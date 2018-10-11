using Newtonsoft.Json;

namespace FinalTaskFacebook.DTOModels
{
    public class PictureFb
    {
        [JsonProperty("data")]
        public PictureInfo PictureInfo { get; set; }
    }
}
