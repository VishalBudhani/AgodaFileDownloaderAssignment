using Df = DownloaderFactory;
using InputFileReader;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SettingsManager;

namespace DownloadManager
{
    /// <summary>
    /// Responsibilities - Manages the download that is requested.
    /// </summary>
    public class ManageDownload
    {
        /// <summary>
        /// Download URL and Download File Name Details
        /// </summary>
        readonly InputReaderProvider inputReaderProvider;
        /// <summary>
        /// Concrete protocol implementation
        /// </summary>
        readonly Df.DownloaderFactory downloaderFactory;
        /// <summary>
        /// Configuration object
        /// </summary>
        readonly ISettings settings;

        public ManageDownload(
            InputReaderProvider inputReaderProvider,
            Df.DownloaderFactory downloaderFactory,
            ISettings settings
        )
        {
            this.inputReaderProvider = inputReaderProvider;
            this.downloaderFactory = downloaderFactory;
            this.settings = settings;
        }

        /// <summary>
        /// Runs the concrete implementation of the protocols
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Run()
        {
            List<UrlAndFileDetails> downloadSourceInformation = inputReaderProvider.GetUrlsAndFileNames();
            List<Df.IDownloader> downloaders = new List<Df.IDownloader>();

            if(downloadSourceInformation != null && downloadSourceInformation.Count > 0)
            {
                foreach (var urlToDownload in downloadSourceInformation)
                {
                    var protocolObject = downloaderFactory.GetDownloader(urlToDownload, settings);
                    if(protocolObject != null)
                    {
                        protocolObject.SetFileDownloadParameters(urlToDownload.GetFileName(), urlToDownload.GetURL(), settings);
                        downloaders.Add(protocolObject);
                    }
                    
                }

                bool result = true;

                await Task.Run(() => Parallel.ForEach(downloaders, x => {
                    if (x != null)
                    {
                        var t = x.DownloadAsync();
                        if (!t.Result)
                        {
                            result = false;
                        }
                    }
                }));
                return result;
            }
            else
            {
                return true;
            }
        }
    }
}
