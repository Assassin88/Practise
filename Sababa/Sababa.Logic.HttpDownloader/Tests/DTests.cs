using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using Sababa.Logic.HttpDownloader.Classes;

namespace Sababa.Logic.HttpDownloader.Tests
{
    [TestFixture]
    class DTests
    {
        private static readonly IDownloadFiles _files = new CurrentDownloadFiles();
        private DownloadClient _downloadClient = new DownloadClient(_files);
        private static string _path = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\DownloadFiles\\";

        [OneTimeSetUp]
        public void Init()
        {

        }

        [OneTimeTearDown]
        public void OnFinish()
        {

        }

        [Test]
        public async Task TestTest()
        {
            string uri = $"https://zabavnik.club/wp-content/uploads/Kartinki_108_03092507.jpg";
            _downloadClient.AddFile("image1.jpg", uri);
            _downloadClient.AddFile("image2.jpg", uri);
            _downloadClient.AddFile("image3.jpg", uri);
            //_downloadClient.SaveCurrentState("JsonSaver.txt");

            // var a = _downloadClient.ReadFromFile<CurrentDownloadFiles>($"{_path}JsonSaver.txt");

            await _downloadClient.StartDownload("image1.jpg");


            await _downloadClient.StartDownload("image2.jpg");
            await _downloadClient.StartDownload("image3.jpg");
            //_downloadClient.SaveCurrentState("JsonSaver.txt");



            var b = await _downloadClient.StartDownload("image1.jpg");

            Assert.IsTrue(b);
        }

    }
}
