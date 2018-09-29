using Newtonsoft.Json;

namespace Sababa.Data.Storage.Classes
{
    public class Document
    {
        public object Value { get; private set; }

        public Document(object value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(Value, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All

            });
        }
    }
}
