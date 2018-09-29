using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using NUnit.Framework;
using Sababa.Data.Storage.Classes;
using Sababa.Data.Storage.Interfaces;

namespace Sababa.Data.Test.StorageTests
{
    [Serializable]
    [TestFixture]
    class FileStorageTests
    {

        private IFileStorage _storage;
        private string _key;
        private int _value;
        private byte[] _parameter;
        private string _path;
        private DateTime _time;

        [SetUp]
        public void Set()
        {
            _storage = StorageFactory.GetFileStorage();
            _key = "key2";
            _value = 3000;
            _parameter = ObjectToByteArray(_value);
            _path = StorageSettings.GetPathDirectory("FileStorage") + _key;
            _time = DateTime.Now.AddMinutes(1);
        }
        public byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public TValue ByteArrayToObject<TValue>(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return (TValue)obj;
            }
        }

        [Test]
        public void TestSave_SendStreamAndSaveValue_CheckValue()
        {
            using (Stream stream = new MemoryStream(ObjectToByteArray(_value)))
            {
                var value = _storage.Save(stream, _key, _time);

                var item = File.ReadAllBytes(_path);

                var result = ByteArrayToObject<int>(item);

                Assert.AreEqual(result, 3000);
                Assert.IsTrue(value);
            }
        }

        [Test]
        public void TestSave_SendNullFileName_ReturnFalse()
        {
            var stream = new MemoryStream();

            var result = _storage.Save(stream, null, _time);

            Assert.IsFalse(result);

            stream.Dispose();
        }

        [Test]
        public void TestSave_SendBytesAndSaveValue_CheckValue()
        {
            var value = _storage.Save(_parameter, _key, _time);

            var result = ByteArrayToObject<int>(File.ReadAllBytes(_path));

            Assert.AreEqual(result, 3000);
            Assert.IsTrue(value);

            value = _storage.Save(_parameter, null, _time);

            Assert.IsFalse(value);
        }

        [Test]
        public async Task TestSaveAsync_SendBytesAndSaveValue_CheckValue()
        {
            var value = await _storage.SaveAsync(_parameter, _key, _time);

            var result = ByteArrayToObject<int>(File.ReadAllBytes(_path));

            Assert.AreEqual(result, 3000);
            Assert.IsTrue(value);

            value = await _storage.SaveAsync(_parameter, null, _time);

            Assert.IsFalse(value);
        }

        [Test]
        public void TestReadAllbytes_ReadBytesFromFile_CheckBytes()
        {
            _storage.Save(_parameter, _key, _time);

            var value = _storage.ReadAllBytes(_key);

            Assert.AreEqual(_parameter, value);

            value = _storage.ReadAllBytes(null);

            Assert.AreEqual(value, new byte[0]);
        }

        [Test]
        public async Task TestReadAllBytesAsync_ReadBytesFromFile_CheckBytes()
        {
            _storage.Save(_parameter, _key, _time);

            var value = await _storage.ReadAllBytesAsync(_key);

            Assert.AreEqual(_parameter, value);

            value = await _storage.ReadAllBytesAsync(null);

            Assert.AreEqual(value, new byte[0]);
        }

        [Test]
        public void TestGetStream_PassingStreamInMethodSave_CheckValue()
        {
            _path = StorageSettings.GetPathDirectory("FileStorage") + "Test";
            File.WriteAllText(_path, _value.ToString());
            var value = _storage.Save(_storage.GetStream("Test"), _key, _time);
            _path = StorageSettings.GetPathDirectory("FileStorage") + _key;
            var item = File.ReadAllText(_path);

            Assert.IsTrue(value);
            Assert.AreEqual(item, _value.ToString());
        }

        [Test]
        public void TestRemove_SaveValueAndRemoveValue_CheckValue()
        {
            var result = _storage.Save(_parameter, _key, _time);

            var value = File.ReadAllBytes(_path);

            Assert.IsTrue(result);
            Assert.AreEqual(_parameter, value);

            result = _storage.Remove(_key);

            Assert.IsTrue(result);

            result = File.Exists(_path);

            Assert.IsFalse(result);
        }

        [Test]
        public void TestClear_ClearStorage_CheckStorage()
        {
            var result = _storage.Save(_parameter, _key, _time);

            Assert.IsTrue(result);

            _storage.Clear();

            var path = StorageSettings.GetPathDirectory("FileStorage");

            var directory = new DirectoryInfo(path);

            var files = directory.GetFiles();

            Assert.AreEqual(files.Length, 0);
        }

        [Test]
        public void TestCount_ClearStorageSaveFile_CheckCount()
        {
            _storage.Clear();

            Assert.AreEqual(_storage.Count(), 0);

            var result = _storage.Save(_parameter, _key, _time);

            Assert.IsTrue(result);
            Assert.AreEqual(_storage.Count(), 1);
        }
    }
}
