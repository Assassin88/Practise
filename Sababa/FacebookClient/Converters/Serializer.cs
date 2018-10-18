using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FacebookClient.Converters
{
    public class Serializer
    {
        public static async Task<T> DeserializeObject<T>(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
