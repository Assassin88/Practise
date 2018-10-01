using System.IO;
using Newtonsoft.Json;

namespace Sababa.logic.HttpClient.Classes
{
    public class JsonSerializeWorker : BaseSerializeWorker
    {
        private static readonly JsonSerializerSettings _jsonSettings =
            new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };

        internal override Stream Serialize(object param)
        {
            var memory = new MemoryStream();
            TextWriter writer = new StreamWriter(memory);
            JsonSerializer.CreateDefault(_jsonSettings).Serialize(writer, param);
            writer.Flush();
            memory.Position = 0;

            return memory;
        }

        internal override T Deserialize<T>(Stream stream)
        {
            using (TextReader reader = new StreamReader(stream))
            {
                var jsonReader = new JsonTextReader(reader);
                return JsonSerializer.CreateDefault(_jsonSettings).Deserialize<T>(jsonReader);
            }
        }

    }
}
