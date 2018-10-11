using Newtonsoft.Json;

namespace FinalTaskFacebook.DTOModels
{
    public class MusicInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
