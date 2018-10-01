using System.IO;

namespace Sababa.logic.HttpClient.Classes
{
    public abstract class BaseSerializeWorker
    {
        internal abstract Stream Serialize(object param);

        internal abstract T Deserialize<T>(Stream stream);
    }
}
