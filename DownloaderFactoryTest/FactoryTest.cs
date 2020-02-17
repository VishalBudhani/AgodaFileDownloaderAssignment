using Df = DownloaderFactory;
using InputFileReader;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using DownloaderFactory;
using SettingsManager;

namespace DownloaderFactoryTest
{
    class FactoryTest
    {
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Converts a valid integer string to integer
        /// </summary>
        [Test]
        public void IntegerValueTestForValidArgument()
        {
            Assert.AreEqual(123456789, FileDownloader.GetIntegerValueFromString("123456789"),
                "GetIntegerValueFromString method is failing for a valid argument");
        }
        /// <summary>
        /// Tries to convert an invalid integer string to integer
        /// </summary>
        [Test]
        public void IntegerValueTestForInValidArgument()
        {
            Assert.Throws(typeof(ArgumentException), new TestDelegate(TestIntegerMethod),
                "GetIntegerValueFromString method is not throwing an exception for an invalid argument");
        }
        /// <summary>
        /// Converts a valid long string to long value
        /// </summary>
        [Test]
        public void LongValueTestForValidArgument()
        {
            Assert.AreEqual(123456789, FileDownloader.GetLongValueFromString("123456789"),
                "GetLongValueFromString method is failing for a valid argument");
        }
        /// <summary>
        /// Tries to convert an invalid long string to long value
        /// </summary>
        [Test]
        public void LongValueTestForInValidArgument()
        {
            Assert.Throws(typeof(ArgumentException), new TestDelegate(TestLongMethod),
                "GetLongValueFromString method is not throwing an exception for an invalid argument");
        }

        public void TestIntegerMethod()
        {
            FileDownloader.GetIntegerValueFromString("abcdef");
        }
        public void TestLongMethod()
        {
            FileDownloader.GetLongValueFromString("abcdef");
        }
        /// <summary>
        /// Checks if the correct protocol is returned for the input string
        /// </summary>
        [Test]
        public void GetDownloaderTest()
        {
            string downloadUrl = "http://www.google.com";
            string fileName = "SampleTest.jpg";

            UrlAndFileDetails obj = new UrlAndFileDetails(downloadUrl, fileName);
            Assert.AreEqual(typeof(HttpFileDownloader), new Df.DownloaderFactory().GetDownloader(obj, SettingsFactory.Settings).GetType());

            downloadUrl = "ftp://site.com/file1";
            fileName = "SampleTest.jpg";
            obj = new UrlAndFileDetails(downloadUrl, fileName);
            Assert.AreEqual(typeof(FtpFileDownloader), new Df.DownloaderFactory().GetDownloader(obj, SettingsFactory.Settings).GetType());

            downloadUrl = "https://www.google.com";
            fileName = "SampleTest.jpg";
            obj = new UrlAndFileDetails(downloadUrl, fileName);
            Assert.AreEqual(typeof(HttpFileDownloader), new Df.DownloaderFactory().GetDownloader(obj, SettingsFactory.Settings).GetType());
        }
    }
}
