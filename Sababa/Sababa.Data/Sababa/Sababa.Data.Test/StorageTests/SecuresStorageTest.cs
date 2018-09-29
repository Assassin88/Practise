using System.Threading.Tasks;
using NUnit.Framework;
using Sababa.Data.Storage.Classes;
using Sababa.Data.Storage.Interfaces;

namespace Sababa.Data.Test.StorageTests
{
    [TestFixture]
    class SecuresStorageTest
    {
        private IKeyValueStorage<string, Document> _storage;
        private string _key;
        private Document _value;

        [SetUp]
        public void Set()
        {
            _storage = StorageFactory.GetSecureStorage();
            _key = "key1";
            _value = new Document(1500);
        }

        [Test]
        public void TestFindOne_GetValue_CheckValue()
        {
            _storage.SaveOrUpdate(_key, _value);

            var result = _storage.FindOne(_key);

            Assert.AreEqual(result.Value, 1500);
        }

        [Test]
        public async Task TestFindOneAsync_GetValue_CheckValue()
        {
            _storage.SaveOrUpdate(_key, _value);

            var result = await _storage.FindOneAsync(_key);

            Assert.AreEqual(result.Value, 1500);
        }

        [Test]
        public void TestSaveOrUpdate_SaveValue_CheckValue()
        {
            _storage.SaveOrUpdate(_key, _value);
            var result = _storage.ContainsKey(_key);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task TestSaveOrUpdateAsync_SaveValue_CheckValue()
        {
            await _storage.SaveOrUpdateAsync(_key, _value);
            var result = _storage.ContainsKey(_key);
            Assert.IsTrue(result);
        }
    }
}
