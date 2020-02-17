using DownloadManager;
using DRunner = DownloadRunner;
using Newtonsoft.Json;
using Serilog;
using SettingsManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileDownloader
{
    public class App
    {
        ISettings _settings;

        public App(ISettings settings)
        {
            _settings = settings;
        }
        public void Run()
        {
            Log.Information($"File Downloader program started...");

            try
            {
                DRunner.DownloadRunner st = new DRunner.DownloadRunner(_settings);
                st.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex, Formatting.Indented));
            }

            Log.Information($"File Downloader program ended.");
        }
    }
}
