using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sababa.logic.HttpClient.Classes
{
    public class BinSerializeWorker : BaseSerializeWorker
    {
        internal override Stream Serialize(object param)
        {
            var memory = new MemoryStream();
            new BinaryFormatter().Serialize(memory, param);
            memory.Position = 0;
            return memory;
        }

        internal override T Deserialize<T>(Stream stream)
        {
            return (T)new BinaryFormatter().Deserialize(stream);
        }

    }
}
