using InputFileReader;
using NUnit.Framework;

namespace InputReaderProviderTest
{
    class ReaderProviderTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestWithCorrectURLAndFileName()
        {
            string downloadUrl = "www.google.com";
            string fileName = "Test File";
            UrlAndFileDetails obj = new UrlAndFileDetails(downloadUrl, fileName);
            Assert.AreEqual(downloadUrl, obj.GetURL());
            Assert.AreEqual(fileName, obj.GetFileName());
        }

        [Test]
        public void TestWithNullURLAndFileName()
        {
            UrlAndFileDetails obj = new UrlAndFileDetails(null, null);
            Assert.AreEqual(null, obj.GetURL());
            Assert.AreEqual(null, obj.GetFileName());
        }
    }
}
