using InputFileReader;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InputReaderProviderTest
{
    class FileReaderTest
    {
        string urlFileContent = string.Empty;
        string filePath = Path.GetTempPath() + "UrlFile.txt";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestWithCorrectFilePathAndAValidFile()
        {
            FileStream fs = null;
            try
            {
                // Setting up 10 urls in the file.
                urlFileContent = string.Empty;
                urlFileContent += "www.google.com/xyz/abctest/123.jpg" + Environment.NewLine;
                urlFileContent += "www.google.com/xyz/abctest/123.jpg" + Environment.NewLine;
                urlFileContent += "www.google.com/xyz/abctest/123.jpg" + Environment.NewLine;
                urlFileContent += "www.google.com/xyz/abctest/123.jpg" + Environment.NewLine;
                urlFileContent += "www.google.com/xyz/abctest/123.jpg" + Environment.NewLine;
                urlFileContent += "www.google.com/xyz/abctest/123.jpg" + Environment.NewLine;
                urlFileContent += "www.google.com/xyz/abctest/123.jpg" + Environment.NewLine;
                urlFileContent += "www.google.com/xyz/abctest/123.jpg" + Environment.NewLine;
                urlFileContent += "www.google.com/xyz/abctest/123.jpg" + Environment.NewLine;
                urlFileContent += "www.google.com/xyz/abctest/123.jpg";

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                fs = File.Create(filePath);
                fs.Close();
                File.WriteAllText(filePath, urlFileContent);

                FileReader obj = new FileReader(filePath);
                var urls = obj.GetUrlsAndFileNames();

                Assert.IsTrue(urls != null && urls.Count > 0);
                Assert.AreEqual(10, urls.Count);

                string[] inputUrls = urlFileContent.Split(Environment.NewLine);
                Assert.AreEqual(inputUrls.Length, urls.Count);
                for (int i = 0; i < urls.Count; i++)
                {
                    Assert.AreEqual(inputUrls[i], urls[i].GetURL());
                }
            }
            finally
            {
                if(fs != null)
                    fs.Close();
            }

        }

        [Test]
        public void TestWithCorrectFilePathAndAValidFileWithSomeInvalidUrls()
        {
            FileStream fs = null;
            try
            {
                urlFileContent = string.Empty;
                // Setting up 10 urls in the file.
                urlFileContent += "www.google.com/xyz/abctest/123.jpg" + Environment.NewLine;
                urlFileContent += "www.google.com/xyz/abctest/123.jpg" + Environment.NewLine;
                urlFileContent += "www.google.com/xyz/abctest/123.jpg" + Environment.NewLine;
                urlFileContent += "www.google.com/xyz/abctest/123.jpg" + Environment.NewLine;
                urlFileContent += Environment.NewLine;
                urlFileContent += Environment.NewLine;
                urlFileContent += Environment.NewLine;
                urlFileContent += "www.google.com/xyz/abctest/123.jpg" + Environment.NewLine;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                fs = File.Create(filePath);
                fs.Close();
                File.WriteAllText(filePath, urlFileContent);

                FileReader obj = new FileReader(filePath);
                var urls = obj.GetUrlsAndFileNames();

                Assert.IsTrue(urls != null && urls.Count > 0);
                Assert.AreEqual(5, urls.Count);

                string[] inputUrls = urlFileContent.Split(Environment.NewLine);
                for (int i = 0; i < urls.Count; i++)
                {
                    if(inputUrls[i].Length > 0)
                        Assert.AreEqual(inputUrls[i], urls[i].GetURL());
                }
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

        }

        [Test]
        public void TestWithIncorrectFilePath()
        {
            FileReader obj = new FileReader(null);
            var urls = obj.GetUrlsAndFileNames();
            Assert.IsTrue(urls == null);
        }

        [TearDown]
        public void CleanUp()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
