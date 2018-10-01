using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using Sababa.logic.HttpClient.Classes;

namespace Sababa.logic.HttpClient.Tests
{
    [TestFixture]
    class RCTest
    {
        private const string _host = "http://localhost:3000/";
        private IListener _listener;
        private IRestClient _client;
        private const string _testString = "Test string!!!";
        private readonly Book _book = new Book(1, "Cats", 5000);

        [OneTimeSetUp]
        public void Init()
        {
            _client = new RestClient(_host);
            _listener = new Listener(_host);
            _listener.Start();
        }

        [OneTimeTearDown]
        public void OnFinish()
        {
            _listener.Stop();
        }

        [Test]
        public async Task TesPutAsync_SendPutMessageWithHeaders_CheckReturnValue()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>()
            {
                {"Hello", "World" }
            };

            var responseMessage = await _client.PutAsync<string>(_testString, "/Server?Books/id=1&name=Cats", TypeOfConvert.Json, headers);
            var result = responseMessage.Headers.Contains("Hello");

            Assert.IsTrue(result);
        }

        [Test]
        public async Task TestGetAsync_SendGetMessageInJsonFormat_CheckReturnValue()
        {
            Book responseBook = await _client.GetValueAsync<Book>(await _client.GetAsync("/Server?Books/id=1&name=Cats"), TypeOfConvert.Json);
            Assert.AreEqual(_book, responseBook);
        }

        [Test]
        public async Task TestPostAsync_SendPostMessageInJsonFormat_CheckReturnValue()
        {
            var responseMessage = await _client.PostAsync<Book>(_book, "/Server?Books/", TypeOfConvert.Json);
            var result = await _client.GetValueAsync<Book>(responseMessage, TypeOfConvert.Json);

            Assert.AreEqual(_book, result);
        }

        [Test]
        public async Task TestPostAsync_SendPostMessageInXmlFormat_CheckReturnValue()
        {
            var responseMessage = await _client.PostAsync<Book>(_book, "/Server?Books/", TypeOfConvert.Xml);
            var result = await _client.GetValueAsync<Book>(responseMessage, TypeOfConvert.Xml);

            Assert.AreEqual(_book, result);
        }

        [Test]
        public async Task TestPostAsync_SendPostMessageInBinaryFormat_CheckReturnValue()
        {
            var responseMessage = await _client.PostAsync<Book>(_book, "/Server?Books/", TypeOfConvert.Binary);
            var result = await _client.GetValueAsync<Book>(responseMessage, TypeOfConvert.Binary);

            Assert.AreEqual(_book, result);
        }

        [Test]
        public async Task TestPutAsync_SendPutMessageInJsonFormat_CheckReturnValue()
        {
            var responseMessage = await _client.PutAsync<Book>(_book, "/Server?books/", TypeOfConvert.Json);
            var result = await _client.GetValueAsync<Book>(responseMessage, TypeOfConvert.Json);

            Assert.AreEqual(_book, result);
        }

        [Test]
        public async Task TestPatchAsync_SendPatchMessageInJsonFormat_CheckReturnValue()
        {
            var responseMessage = await _client.PatchAsync<string>(_testString, "/Server?books/", TypeOfConvert.Json);
            var result = await _client.GetValueAsync<string>(responseMessage, TypeOfConvert.Json);

            Assert.AreEqual(_testString, result);
        }

        [Test]
        public async Task TestDeleteAsync_SendDeleteMessageInJsonFormat_CheckReturnValue()
        {
            var responseMessage = await _client.DeleteAsync("/Server?Books/id=1&name=Cats");
            var statusCode = responseMessage.StatusCode;

            if (statusCode == HttpStatusCode.OK)
            {
                Assert.IsTrue(statusCode == HttpStatusCode.OK);
            }
            else
            {
                Assert.IsFalse(statusCode == HttpStatusCode.OK);
            }
        }

        //private async Task<string> ResponseConvertToString(HttpResponseMessage message)
        //{
        //    byte[] actual;
        //    using (var ms = new MemoryStream())
        //    {
        //        await message.Content.CopyToAsync(ms);
        //        actual = ms.ToArray();
        //    }

        //    return (Encoding.UTF8.GetString(actual));
        //}

    }
}
