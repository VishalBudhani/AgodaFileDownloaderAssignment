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
    public class FtpFileDownloader : IDownloader
    {
        string downloadFileName;
        string urlToDownloadFrom;
        ISettings settings;
        string ftpUserName;
        string ftpPassword;

        public void SetFileDownloadParameters(string downloadFileName, string urlToDownloadFrom, ISettings settings)
        {
            this.downloadFileName = downloadFileName;
            this.urlToDownloadFrom = urlToDownloadFrom;
            this.settings = settings;
            this.ftpUserName = settings.FtpUserName;
            this.ftpPassword = settings.FtpPassword;
        }

        /// <summary>
        /// Downloads the payload asynchronously 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DownloadAsync()
        {
            try
            {
                //await RetryFailedDownload.RetryWhenFail<HttpRequestException>(settings.MaximumRetry,
                // settings.TimeGapBetweenRetires,
                // async () =>
                // {
                //     await DownloadFTPFile(urlToDownloadFrom, settings.FileDownloadLocation, downloadFileName,
                //         RetryFailedDownload.GetIntegerValueFromString(settings.BlockSizeForDownload));
                // });
                await DownloadFTPFile(urlToDownloadFrom, settings.FileDownloadLocation, downloadFileName,
                         FileDownloader.GetIntegerValueFromString(settings.BlockSizeForDownload),
                         FileDownloader.GetIntegerValueFromString(settings.MaximumRetry));
            }
            catch
            {
                return false;
            }
            return true;            
        }
        /// <summary>
        /// This is the implementation for the ftp file download only
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <param name="downloadLocation"></param>
        /// <param name="fileName"></param>
        /// <param name="blockSize"></param>
        /// <returns></returns>
        private async Task<bool> DownloadFTPFile(string fileUrl, string downloadLocation, string fileName, int blockSize, int maxRetry)
        {
            try
            {
                Log.Information("Downloading file with file name: {0} using ftp protocol from the url: {1} and saving it to the location: {2}",
               fileName, fileUrl, downloadLocation);
                if (!string.IsNullOrEmpty(fileUrl) && fileUrl.Contains("ftp"))
                {

                    if (!Directory.Exists(downloadLocation))
                    {
                        Log.Error("Download folder doesn't exist. Please provide a download folder.");
                        return false;
                    }

                    try
                    {
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fileUrl);
                        request.Method = WebRequestMethods.Ftp.DownloadFile;
                        request.KeepAlive = true;
                        request.UsePassive = false;
                        request.UseBinary = (fileUrl.Contains(".zip") ? true : false);

                        request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                        request.Timeout = FileDownloader.GetIntegerValueFromString(settings.RequestTimeout);

                        Log.Information("Getting response for the ftp url");

                        using (var response = (FtpWebResponse)request.GetResponse())
                        {
                            Log.Information("Response for the http url received with a content length {0}", response.ContentLength);
                            if (response.ContentLength > 0)
                            {
                                using (var responseStream = response.GetResponseStream())
                                {
                                    Log.Information("Response for the ftp url received with a content length {0}", response.ContentLength);
                                    if (response.ContentLength > 0)
                                    {
                                        FileDownloader downloader = new FileDownloader();
                                        var result = await downloader.DownloadAndSaveFile(response.ContentLength, responseStream, 
                                            blockSize, downloadLocation + fileName, maxRetry);
                                        return result.IsSuccess;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        Log.Error("An error occured in the DownloadFTPFile subroutine for the file {0}", fileName);
                        return false;
                    }
                    finally
                    {

                    }
                }
                Log.Information("File {0} downloaded successfully", fileName);
                return true;
            }
            catch
            {
                Log.Error("An error occured in the DownloadFTPFile subroutine");
                return false;
            }
        }
    }
}
