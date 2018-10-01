using System.IO;
using System.Xml.Serialization;

namespace Sababa.logic.HttpClient.Classes
{
    internal class XmlSerializeWorker : BaseSerializeWorker
    {
        internal override Stream Serialize(object param)
        {
            var memory = new MemoryStream();
            new XmlSerializer(param.GetType()).Serialize(memory, param);
            memory.Position = 0;
            return memory;
        }

        internal override T Deserialize<T>(Stream stream)
        {
            return (T)new XmlSerializer(typeof(T)).Deserialize(stream);
        }

    }
}
