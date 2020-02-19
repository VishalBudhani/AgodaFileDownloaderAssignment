using Dr = DownloadRunner;
using NUnit.Framework;
using SettingsManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Moq;
using DownloaderFactory;

namespace DownloadRunnerTest
{
    class DownloadRunnerTest
    {
        ISettings settings;
       [SetUp]
        public void Setup()
        {
            settings = SettingsFactory.Settings;
            settings.FileDownloadLocation = Path.GetTempPath() + @"DownloadedFiles\";
        }

        /// <summary>
        /// Runs the test with a valid UrlInputFile.
        /// Should download images in the %temp%\DownloadedFiles\ location
        /// </summary>
        [Test]
        public void RunnerTestWithValidUrls()
        {
            // Provide the correct URL file location for the test
            settings.UrlInformationFile = @"E:\\DownloadFilesOutput\\URLFolder\\UrlInformation.txt";

            Assert.IsNotNull(settings);
            if (Directory.Exists(settings.FileDownloadLocation))
            {
                Directory.Delete(settings.FileDownloadLocation, true);
            }

            Directory.CreateDirectory(settings.FileDownloadLocation);
            Dr.DownloadRunner obj = new Dr.DownloadRunner(settings);
            obj.Run();

            Assert.Greater(Directory.GetFiles(settings.FileDownloadLocation).Length, 0, "Files didnot download for the correct urls");

        }

        /// <summary>
        /// UrlFile path will be changed to an invalid path so that the total files downloaded will be zero
        /// </summary>
        [Test]
        public void RunnerTestWithInValidUrls()
        {
            Assert.IsNotNull(settings);
            if (Directory.Exists(settings.FileDownloadLocation))
            {
                Directory.Delete(settings.FileDownloadLocation, true);
            }

            Directory.CreateDirectory(settings.FileDownloadLocation);
            //changing the file path 
            settings.UrlInformationFile = ""; 

            Dr.DownloadRunner obj = new Dr.DownloadRunner(settings);
            obj.Run();

            Assert.AreEqual(Directory.GetFiles(settings.FileDownloadLocation).Length, 0, "File count in the download directory is not zero");

        }

        /// <summary>
        /// Tests the number of retries whenever an exception is encountered while downloading
        /// a file
        /// </summary>
        [Test]
        public void RetryCountTest()
        {
            int retryCount = 3;
            var downloaderObj = new FileDownloader();
            var result = downloaderObj.DownloadAndSaveFile(0, null, 0, null, retryCount);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result.IsSuccess);
            Assert.AreEqual(retryCount, result.Result.RetryCount);
        }

        [TearDown]
        public void DeleteDownloadFolder()
        {
            if (Directory.Exists(settings.FileDownloadLocation))
            {
                Directory.Delete(settings.FileDownloadLocation, true);
            }
        }
    }
}
