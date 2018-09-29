using NUnit.Framework;
using Sababa.Crosscutting.Caching;

namespace Sababa.Logic.Tests
{
    [TestFixture]
    public class CacheUnitTests
    {
        private ICache _cache;
        private string _key;
        private string _value;

        [SetUp]
        public void SetUp()
        {
            _cache = new Cache();
            _key = "key1";
            _value = "value1";
        }

        [Test]
        public void AddAndGetItem_AddItemsToCacheAndThenGetIt_ReturnInsertedItem()
        {
            _cache.AddItem(_key, _value);

            Assert.That(_cache.GetItem<string>(_key), Is.EqualTo(_value));
        }

        [Test]
        public void IsExistKey_AddItemdToCacheAndThenCheckIfItExists_ReturnTrue()
        {
            _cache.AddItem(_key, _value);

            Assert.IsTrue(_cache.IsExistKey(_key));
            Assert.IsFalse(_cache.IsExistKey("key2"));
        }

        [Test]
        public void RemoveItem_AddItemToTheCacheThenRemoveAndCheckIfExists_ReturnFalse()
        {
            _cache.AddItem(_key, _value);
            Assert.IsTrue(_cache.IsExistKey(_key));

            _cache.RemoveItem(_key);
            Assert.IsFalse(_cache.IsExistKey(_key));
        }

    }
}
