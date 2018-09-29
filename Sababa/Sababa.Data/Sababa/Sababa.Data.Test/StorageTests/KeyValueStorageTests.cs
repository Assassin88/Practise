using System.Threading.Tasks;
using NUnit.Framework;
using Sababa.Data.Storage.Classes;
using Sababa.Data.Storage.Interfaces;

namespace Sababa.Data.Test.StorageTests
{
    [TestFixture]
    class KeyValueStorageTests
    {
        private IKeyValueStorage<string, Document> _storage;
        private string _key;
        private Document _value;

        [SetUp]
        public void Set()
        {
            _storage = StorageFactory.GetKeyValueStorage();
            _key = "key";
            _value = new Document(1000);
        }

        [Test]
        public void TestSaveOrupdate_AddValue_CheckValue()
        {
            _storage.SaveOrUpdate(_key, _value);

            var result = _storage.ContainsKey(_key);

            Assert.IsTrue(result);
        }

        [Test]
        public void TestSaveOrupdate_UpdateValue_CheckValue()
        {
            _storage.SaveOrUpdate(_key, _value);
            var result = _storage.ContainsKey(_key);
            Assert.IsTrue(result);

            _value = new Document(500);
            _storage.SaveOrUpdate(_key, _value);
            var newresult = _storage.FindOne(_key);
            Assert.AreEqual(newresult.Value, 500);
        }

        [Test]
        public async Task TestSaveOrupdateAsunc_AddValue_CheckValue()
        {
            await _storage.SaveOrUpdateAsync(_key, _value);

            var result = _storage.ContainsKey(_key);

            Assert.IsTrue(result);

            _value = new Document(500);
            await _storage.SaveOrUpdateAsync(_key, _value);
            var newresult = await _storage.FindOneAsync(_key);
            Assert.AreEqual(newresult.Value, 500);
        }

        [Test]
        public async Task TestSaveOrupdateAsunc_UpdateValue_CheckValue()
        {
            await _storage.SaveOrUpdateAsync(_key, _value);
            var result = _storage.ContainsKey(_key);
            Assert.IsTrue(result);

            _value = new Document(500);
            await _storage.SaveOrUpdateAsync(_key, _value);
            var newresult = _storage.FindOne(_key);
            Assert.AreEqual(newresult.Value, 500);
        }

        [Test]
        public void TestFindOne_GetValue_CheckValue()
        {
            _storage.SaveOrUpdate(_key, _value);

            var result = _storage.FindOne(_key);

            Assert.AreEqual(result.Value, 1000);
        }

        [Test]
        public void TestFindOne_SendNonexistentKey_ReturnEmptyDocument()
        {
            var result = _storage.FindOne("key2");

            Assert.AreEqual(result.Value, null);
        }

        [Test]
        public async Task TestFindOneAsync_GetValue_Check_Value()
        {
            _storage.SaveOrUpdate(_key, _value);

            var result = await _storage.FindOneAsync(_key);

            Assert.AreEqual(result.Value, 1000);
        }

        [Test]
        public void TestRemove_RemoveValue_CheckValue()
        {
            _storage.SaveOrUpdate(_key, _value);

            _storage.Remove(_key);

            var result = _storage.ContainsKey(_key);

            Assert.IsFalse(result);
        }

        [Test]
        public void TestCount_AddValue_CheckCount()
        {
            var count = _storage.Count();

            _storage.SaveOrUpdate("key3", new Document(666));

            Assert.AreNotEqual(count, _storage.Count());
        }

        [Test]
        public void TestContainsKey_AddKey_CheckKey()
        {
            _storage.SaveOrUpdate(_key, _value);

            var result = _storage.ContainsKey(_key);

            Assert.IsTrue(result);
        }

        [Test]
        public void TestClear_ClearStorageAndDictionary_CheckStorageAndDictionary()
        {
            _storage.SaveOrUpdate(_key, _value);

            _storage.Clear();

            var result = _storage.ContainsKey(_key);

            Assert.IsFalse(result);

            var value = _storage.FindOne(_key);

            Assert.AreEqual(value.Value, null);
        }

        [Test]
        public void TestIsEmpty_ClearStorage_CheckIsEmptyStorage()
        {
            _storage.Clear();

            var result = _storage.IsEmpty();

            Assert.IsTrue(result);
        }
    }
}
