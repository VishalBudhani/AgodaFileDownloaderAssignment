using NUnit.Framework;
using SettingsManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SettingsManagerTest
{
    class AppsetingsConfigTest
    {
        /// <summary>
        /// Test to check if the config values are correct
        /// </summary>
        [Test]
        public void ConfigValuesTest()
        {
            ISettings configValues = SettingsFactory.Settings;

            Assert.IsNotNull(configValues, "Config values are null or empty");
            Assert.IsNotNull(configValues.FileDownloadLocation, "File Download Location config value is null");
            Assert.IsNotEmpty(configValues.FileDownloadLocation, "File Download Location config value is empty");
            Assert.IsTrue(Directory.Exists(configValues.FileDownloadLocation), "File Download directory doesn't exist");

            Assert.IsNotNull(configValues.UrlInformationFile, "URLInformation file path is null");
            Assert.IsNotEmpty(configValues.UrlInformationFile, "URLInformation file path is empty");
            Assert.IsTrue(File.Exists(configValues.UrlInformationFile), "URLInformation file doesn't exist");

            int integerValue = 0;
            Assert.IsTrue(int.TryParse(configValues.BlockSizeForDownload, out integerValue), "Download block size value is not an integer value");
            Assert.IsTrue(int.TryParse(configValues.MaxFileNameLength, out integerValue), "MaxFileNameLength value is not an integer value");
            Assert.IsTrue(int.TryParse(configValues.MaximumRetry, out integerValue), "MaximumRetry value is not an integer value");
            Assert.IsTrue(int.TryParse(configValues.TimeGapBetweenRetires, out integerValue), "TimeGapBetweenRetires value is not an integer value");

        }
    }
}
