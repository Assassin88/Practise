using System.Collections.Generic;
using Newtonsoft.Json;

namespace FinalTaskFacebook.DTOModels
{
    public class MusicFb
    {
        [JsonProperty("data")]
        public List<MusicInfo> MusicInfo { get; set; }
    }
}
