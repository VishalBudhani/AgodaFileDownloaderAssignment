using System;
using System.Collections.Generic;
using System.Text;

namespace InputFileReader
{
    /// <summary>
    /// Responsibilities - Knows how to read the requested file Urls from various sources.
    /// </summary>
    public abstract class InputReaderProvider
    {
        public abstract List<UrlAndFileDetails> GetUrlsAndFileNames();
    }

    public class UrlAndFileDetails
    {
        /// <summary>
        /// url to download from
        /// </summary>
        string downloadURL;
        /// <summary>
        /// Name of the file to be saved
        /// </summary>
        string fileName;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="downloadURL"></param>
        /// <param name="fileName"></param>
        public UrlAndFileDetails(string downloadURL, string fileName)
        {
            this.downloadURL = downloadURL;
            this.fileName = fileName;
        }
        /// <summary>
        /// Returns the file name
        /// </summary>
        /// <returns></returns>
        public string GetFileName()
        {
            return fileName;
        }
        /// <summary>
        /// Returns the URL
        /// </summary>
        /// <returns></returns>
        public string GetURL()
        {
            return downloadURL;
        }
    }
}
