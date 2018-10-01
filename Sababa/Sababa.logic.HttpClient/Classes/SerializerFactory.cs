using System;
using System.Collections.Generic;

namespace Sababa.logic.HttpClient.Classes
{
    internal class SerializerFactory
    {
        private static readonly Dictionary<TypeOfConvert, BaseSerializeWorker> _workers =
            new Dictionary<TypeOfConvert, BaseSerializeWorker>();

        static SerializerFactory()
        {
            _workers.Add(TypeOfConvert.Json, new JsonSerializeWorker());
            _workers.Add(TypeOfConvert.Xml, new XmlSerializeWorker());
            _workers.Add(TypeOfConvert.Binary, new BinSerializeWorker());
        }

        internal static BaseSerializeWorker GetSerializer(TypeOfConvert type)
        {
            if (!_workers.ContainsKey(type))
            {
                throw new ArgumentException("No such SerializeWorker type exists!");
            }

            return _workers[type];
        }
    }
}
