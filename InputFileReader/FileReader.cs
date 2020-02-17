using SettingsManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InputFileReader
{
    /// <summary>
    /// Responsibility Concrete implementation of reading the URL Information and 
    /// and the DownloadFile names from the input UrlInformation file
    /// </summary>
    public class FileReader : InputReaderProvider
    {
        readonly string filepath;

        public FileReader(string filepath)
        {
            this.filepath = filepath;
        }

        /// <summary>
        /// Returns the Urls and the file names to download from
        /// </summary>
        /// <returns></returns>
        public override List<UrlAndFileDetails> GetUrlsAndFileNames()
        {
            List<UrlAndFileDetails> result = null;
            int maxLengthOfFileName = Convert.ToInt32(SettingsFactory.Settings.MaxFileNameLength);
            if (File.Exists(filepath))
            {
                string fileText = File.ReadAllText(filepath);
                string[] urls = fileText.Split(Environment.NewLine);
                foreach (var item in urls)
                {
                    if (item.Length > 0)
                    {
                        result = (result == null) ? new List<UrlAndFileDetails>() : result;

                        string downloadSourceURl = item;
                        string downloadedFileName = item.Substring(item.LastIndexOf("/") + 1, item.Length - (item.LastIndexOf("/") + 1));
                        
                        //Removing invalid characters from the file name
                        downloadedFileName = string.Join("", downloadedFileName.Split(Path.GetInvalidFileNameChars()));

                        if(downloadedFileName.Length > maxLengthOfFileName)
                        {
                            downloadedFileName = downloadedFileName.Substring(downloadedFileName.Length - maxLengthOfFileName, maxLengthOfFileName);
                        }

                        result.Add(new UrlAndFileDetails(downloadSourceURl, downloadedFileName));
                    }
                }
            }
            else
            {
                return null;
            }
            return result;
        }
    }
}
