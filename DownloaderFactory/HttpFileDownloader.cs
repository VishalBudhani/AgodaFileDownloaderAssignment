using Serilog;
using SettingsManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DownloaderFactory
{
    public class HttpFileDownloader : IDownloader
    {
        string downloadFileName;
        string urlToDownloadFrom;
        ISettings settings;

        public void SetFileDownloadParameters(string downloadFileName, string urlToDownloadFrom, ISettings settings)
        {
            this.downloadFileName = downloadFileName;
            this.urlToDownloadFrom = urlToDownloadFrom;
            this.settings = settings;
        }
        public async Task<bool> DownloadAsync()
        {
            try
            {
                //await RetryFailedDownload.RetryWhenFail<HttpRequestException>(settings.MaximumRetry,
                //    settings.TimeGapBetweenRetires,
                //    async () =>
                //    {
                //        await DownloadHTTPFile(urlToDownloadFrom, settings.FileDownloadLocation, downloadFileName,
                //            RetryFailedDownload.GetIntegerValueFromString(settings.BlockSizeForDownload));
                //    });
                await DownloadHTTPFile(urlToDownloadFrom, settings.FileDownloadLocation, downloadFileName,
                            FileDownloader.GetIntegerValueFromString(settings.BlockSizeForDownload),
                            FileDownloader.GetIntegerValueFromString(settings.MaximumRetry));
            }
            catch
            {
                return false;
            }
            return true;
        }

        private async Task<bool> DownloadHTTPFile(string fileUrl, string downloadLocation, string fileName, int blockSize, int maxRetry)
        {
            try
            {
                Log.Information("Downloading file with file name: {0} using http protocol from the url: {1} and saving it to the location: {2}",
                    fileName, fileUrl, downloadLocation);
                long totalFileSizeToDownload = GetFileSize(fileUrl);
                Log.Information("Total download size for the file in bytes is: {0}", totalFileSizeToDownload);

                if(!Directory.Exists(downloadLocation))
                {
                    Log.Error("Download folder doesn't exist. Please provide a download folder.");
                    return false;
                }

                var request = WebRequest.Create(fileUrl);
                request.Timeout = FileDownloader.GetIntegerValueFromString(settings.RequestTimeout);
                Log.Information("Getting response for the http url");
                string filePath = downloadLocation + fileName;

                using (var response = request.GetResponse())
                {
                    Log.Information("Response for the http url received with a content length {0}", response.ContentLength);
                    if (response.ContentLength > 0)
                    {
                        using (var responseStream = response.GetResponseStream())
                        {
                            FileDownloader downloader = new FileDownloader();
                            var result = await downloader.DownloadAndSaveFile(response.ContentLength, responseStream, blockSize, filePath, maxRetry);
                            return result.IsSuccess;
                        }
                    }

                }

                Log.Information("File {0} downloaded successfully", fileName);
                return true;
            }
            catch
            {
                Log.Error("An error occured in the DownloadHTTPFile subroutine");
                throw;
            }
        }

        private static long GetFileSize(string fileUrl)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(fileUrl);
                req.Method = "GET";
                using (var webResponse = req.GetResponseAsync())
                {
                    if (long.TryParse(webResponse.Result.Headers.Get("Content-Length"), out long ContentLength))
                    {
                        return ContentLength;
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error("An error occured in the GetFileSize subroutine");
                throw;
            }

        }
    }
}
