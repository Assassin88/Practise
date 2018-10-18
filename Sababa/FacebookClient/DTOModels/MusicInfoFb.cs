using Newtonsoft.Json;

namespace FacebookClient.DTOModels
{
    public class MusicInfoFb
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
