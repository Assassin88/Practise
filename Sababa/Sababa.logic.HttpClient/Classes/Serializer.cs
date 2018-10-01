using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security;

namespace Sababa.logic.HttpClient.Classes
{
    internal class Serializer
    {
        internal static Stream Serialize(object param, TypeOfConvert typeOfConvert)
        {
            try
            {
                var serializer = SerializerFactory.GetSerializer(typeOfConvert);
                return serializer.Serialize(param);
            }
            catch (ArgumentNullException)
            {
                //log
                throw;
            }
            catch (SerializationException)
            {
                //log
                throw;
            }
            catch (SecurityException)
            {
                //log
                throw;
            }
        }

        internal static T Deserialize<T>(Stream stream, TypeOfConvert typeOfConvert)
        {
            try
            {
                var serializer = SerializerFactory.GetSerializer(typeOfConvert);
                return serializer.Deserialize<T>(stream);
            }
            catch (ArgumentNullException)
            {
                //log
                throw;
            }
            catch (SerializationException)
            {
                //log
                throw;
            }
            catch (SecurityException)
            {
                //log
                throw;
            }
        }

    }
}
