using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FinalTaskFacebook.Models
{
    internal class Serializer
    {
        internal static async Task<T> DeserializeObject<T>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
    }
}
