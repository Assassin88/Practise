using NUnit.Framework;
using Sababa.Data.Storage.Classes;

namespace Sababa.Data.Test.StorageTests
{
    [TestFixture]
    class TestsCrypt
    {
        private Crypt _crypt;
        private byte[] _bytes;

        [SetUp]
        public void Set()
        {
            _crypt = new Crypt("123");
            _bytes = new byte[] { 1, 2, 3, 4, 5 };
        }

        [Test]
        public void TestEncrypt_SendBytesEncryptBytes_CheckBytes()
        {
            var value = _crypt.Encrypt(_bytes);

            Assert.AreNotEqual(value, _bytes);
        }

        [Test]
        public void TestDencrypt_SendBytesEncryptBytes_CheckBytes()
        {
            var value = _crypt.Encrypt(_bytes);

            var result = _crypt.Decrypt(value);

            Assert.AreEqual(result, _bytes);
        }
    }
}
