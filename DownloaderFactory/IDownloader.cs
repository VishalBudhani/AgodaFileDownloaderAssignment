using SettingsManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DownloaderFactory
{
    /// <summary>
    /// Responsibilities - For downloading the file from the input url.
    /// </summary>
    public interface IDownloader
    {
        Task<bool> DownloadAsync();
        void SetFileDownloadParameters(string downloadFileName, string urlToDownloadFrom, ISettings settings);
    }


}
