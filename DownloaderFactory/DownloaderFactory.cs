using InputFileReader;
using Serilog;
using SettingsManager;
using System;
using System.Collections.Generic;

namespace DownloaderFactory
{
    /// <summary>
    /// Responsibilities - Factory that creates the specific downloader based on the url protocol.
    /// </summary>
    public class DownloaderFactory
    {
        /// <summary>
        /// Constructor
        /// Takes the UrlAndFileDetials and settings object
        /// </summary>
        /// <param name="details"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public IDownloader GetDownloader(UrlAndFileDetails details, ISettings settings)
        {
            string downloadUrl = string.Empty;
            if(details != null && details.GetURL() != null)
            {
                Log.Information("Getting download URLS information");

                downloadUrl = details.GetURL();
                if (!string.IsNullOrEmpty(downloadUrl) && downloadUrl.Length > 3)
                {
                    int firstIndex = downloadUrl.IndexOf(":");
                    if(firstIndex > -1)
                    {
                        return new ProtocolIdentifier().GetProtocol(downloadUrl.Substring(0, firstIndex).ToLower());
                    }
                    else
                    {
                        return null;
                    }                    
                }
            }
            return null;
        }
    }

    public class ProtocolIdentifier
    {
        static Dictionary<string, System.Type> protocolTypes = new Dictionary<string, System.Type>();
        public ProtocolIdentifier()
        {
            if(protocolTypes.Count == 0)
            {
                protocolTypes.Add("HTTP", typeof(HttpFileDownloader));
                protocolTypes.Add("HTTPS", typeof(HttpFileDownloader));
                protocolTypes.Add("FTP", typeof(FtpFileDownloader));
            }
        }

        public IDownloader GetProtocol(string protocolType)
        {
            if (!string.IsNullOrEmpty(protocolType) && protocolTypes.ContainsKey(protocolType.ToUpper()))
            {
                return (IDownloader)Activator.CreateInstance(protocolTypes[protocolType.ToUpper()]);
            }
            return null;
        }

    }
}
