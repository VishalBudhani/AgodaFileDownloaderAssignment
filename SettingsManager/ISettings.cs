using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsManager
{
    /// <summary>
    /// Configuration values mentioned in the appsettings, appsettings.Testing & appsettings.Production.json files
    /// </summary>
    public interface ISettings
    {
        string Environment { get; set; }
        string FileDownloadLocation { get; set; }
        string UrlInformationFile { get; set; }
        string MaximumRetry { get; set; }
        string RequestTimeout { get; set; }
        string BlockSizeForDownload { get; set; }
        string MaxFileNameLength { get; set; }
        string FtpUserName { get; set; }
        string FtpPassword { get; set; }
       
    }
}
