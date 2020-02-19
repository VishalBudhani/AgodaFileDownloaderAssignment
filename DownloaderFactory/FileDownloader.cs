using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DownloaderFactory
{
    public class FileDownloader
    {
        public async Task<FileDownloadResult> DownloadAndSaveFile(long contentLength, 
            Stream responseStream, int blockSize, string filePath, int maxRetries)
        {
            long contentLengthToDownload = 0;
            int retryNumber = 1;
            bool hasErrors = true;
            for (retryNumber = 1; retryNumber <= maxRetries; retryNumber++)
            {
                try
                {
                    contentLengthToDownload = contentLength;
                    Log.Information("Total download size for the file in bytes is: {0}", contentLengthToDownload);
                    byte[] buffer = new byte[blockSize];
                    int bytesRead = 0;

                    using (var outputFileStream = File.Create(filePath, blockSize))
                    {
                        do
                        {
                            bytesRead = await responseStream.ReadAsync(buffer, 0, blockSize);
                            outputFileStream.Write(buffer, 0, bytesRead);
                            contentLengthToDownload -= bytesRead;
                        } while (bytesRead > 0);                        
                    }
                    hasErrors = false;
                    break;
                }
                catch (Exception ex)
                {
                    Log.Error("An error occurred in the DownloadAndSaveFile method. Error details are :" + 
                        Environment.NewLine + ex.ToString());
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }

            if(contentLength > 0 && contentLengthToDownload != 0)
            {
                Log.Information("Deleting the partially downloaded file");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return new FileDownloadResult()
                {
                    IsSuccess = !hasErrors,
                    RetryCount = retryNumber-1
                };
            }
            return new FileDownloadResult
            {
                IsSuccess = !hasErrors,
                RetryCount = retryNumber-1
            };
        }

        public class FileDownloadResult
        {
            public bool IsSuccess { get; set; }
            public int RetryCount { get; set; }
        }

        public static int GetIntegerValueFromString(string inputValue)
        {
            int result;
            if (int.TryParse(inputValue, out result))
            {
                return result;
            }
            throw new ArgumentException();
        }
        /// <summary>
        /// Converts a string value to it's equivalent long value
        /// Throws an argument exception if the string provided is not valid
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static long GetLongValueFromString(string inputValue)
        {
            long result;
            if (long.TryParse(inputValue, out result))
            {
                return result;
            }
            throw new ArgumentException();
        }
    }
}
