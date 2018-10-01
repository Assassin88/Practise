using System.IO;
using Newtonsoft.Json;

namespace Sababa.Logic.HttpDownloader.Classes
{
    internal class Serializer
    {
        internal static void Serialize(object param, string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(param, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            }));
        }

        internal static T Deserialize<T>(string path)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path), new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }
    }
}
