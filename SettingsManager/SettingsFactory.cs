using Config.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SettingsManager
{
    /// <summary>
    /// Singleton implemntation of the config values.
    /// </summary>
    public static class SettingsFactory
    {
        private static ISettings settings = null;
        public static ISettings Settings
        {
            get
            {
                if (settings == null)
                {
                    string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                    settings = new ConfigurationBuilder<ISettings>()
                               .UseJsonFile(Path.Combine(basePath, "appsettings.json"))
                               .Build();
                    settings = new ConfigurationBuilder<ISettings>()
                        .UseJsonFile(Path.Combine(basePath, "appsettings.json"))
                        .UseJsonFile(Path.Combine(basePath, $"appsettings.{settings.Environment}.json"))
                        .Build();
                }
                return settings;
            }
        }
    }

}
