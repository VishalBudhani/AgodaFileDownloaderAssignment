using Df= DownloaderFactory;
using InputFileReader;
using SettingsManager;
using System;
using System.Collections.Generic;
using System.Text;
using DownloadManager;
using Serilog;

namespace DownloadRunner
{
    public class DownloadRunner
    {
        /// <summary>
        /// config value object
        /// </summary>
        ISettings settings;

        public DownloadRunner(ISettings settings)
        {
            this.settings = settings;
        }

        public void Run()
        {
            InputReaderProvider inputReaderProvider = GetInputReaderProvider();
            Df.DownloaderFactory downloaderFactory = new Df.DownloaderFactory();
            ManageDownload downloadManager = new ManageDownload(inputReaderProvider, downloaderFactory, settings);
            var task = downloadManager.Run();
            if (task.Result)
            {
                Log.Information("All files downloaded successfully.");
            }
            else
            {
                Log.Information("Downloading of all or some files failed. Please check the logs.");
            }
        }

        private InputReaderProvider GetInputReaderProvider()
        {
            return new FileReader(settings.UrlInformationFile);
        }
    }
}
