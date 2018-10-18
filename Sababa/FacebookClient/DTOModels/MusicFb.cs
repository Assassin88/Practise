using System.Collections.Generic;
using Newtonsoft.Json;

namespace FacebookClient.DTOModels
{
    public class MusicFb
    {
        [JsonProperty("data")]
        public List<MusicInfoFb> MusicInfo { get; set; }
    }
}